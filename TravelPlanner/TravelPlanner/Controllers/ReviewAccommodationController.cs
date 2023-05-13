using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelPlanner.Models;

namespace TravelPlanner.Controllers
{
    public class ReviewAccommodationController : Controller
    {
        string connectionString = "Data Source=DESKTOP-6A1HP7T;Initial Catalog=TravelPlanner;Integrated Security=True";
        // GET: ReviewAccommodation
        public ActionResult Index()
        {
            return View();
        }



        [HttpPost]
        public ActionResult CreateReview(ReviewAccommodation review, int accommodationId)
        {
            LeaveReview(review, accommodationId);

            return Content("Added review");
        }



        public void LeaveReview(ReviewAccommodation review,int accomodationId)
        {
            int userId = (int)Session["UserId"];
          

            if (ModelState.IsValid)
            {
                review.CreatedAt = DateTime.Now;
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string queryToInsertReview = "INSERT INTO ReviewsAccommodations(UserId, AccommodationId, Rating, Comment, CreatedAt) VALUES (@UserId, @AccommodationId, @Rating, @Comment, @CreatedAt)";
                        using (SqlCommand command = new SqlCommand(queryToInsertReview, conn))
                        {
                            command.Parameters.AddWithValue("@UserId", userId);
                            command.Parameters.AddWithValue("@AccommodationId", accomodationId);
                            command.Parameters.AddWithValue("@Rating", review.Rating);
                           command.Parameters.AddWithValue("@Comment", review.Comment);
                        //  command.Parameters.AddWithValue("@Comment", ".....");
                            command.Parameters.AddWithValue("@CreatedAt", review.CreatedAt);

                            command.ExecuteNonQuery();
                        }
                    }
                   
                }
                catch (Exception e)
                {
                    
                }
            }
           
        }


       
       /*
        [HttpPost]

        public ActionResult LeaveReview(ReviewAccommodation review, int accommodationId)
        {
            
          
            if (ModelState.IsValid)
            {

                review.CreatedAt = DateTime.Now;
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string queryToInsertReview = "INSERT INTO ReviewsAccommodations(UserId, AccommodationId, Rating, Comment, CreatedAt) VALUES (@UserId, @AccommodationId, @Rating, @Comment, @CreatedAt)";
                        using (SqlCommand command = new SqlCommand(queryToInsertReview, conn))
                        {
                            int userId = (int)Session["UserId"];
                            command.Parameters.AddWithValue("@UserId", userId);
                            command.Parameters.AddWithValue("@AccommodationId", accommodationId);
                            command.Parameters.AddWithValue("@Rating", review.Rating);
                            // command.Parameters.AddWithValue("@Comment", review.Comment);
                            command.Parameters.AddWithValue("@Comment", ".....");
                            command.Parameters.AddWithValue("@CreatedAt", review.CreatedAt);

                            command.ExecuteNonQuery();
                        }
                    }

                }
                catch (Exception e)
                {

                }
            }
          
            return Content("Added review"); ;


           
        }
       */

        [HttpPost]
        public ActionResult DisplayReviewAccommodation(int accommodationId)
        {
            List<ReviewAccommodation> reviewAccommodations = new List<ReviewAccommodation>();

            string name = Session["firstname"] as string;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string queryToDisplayReview = "SELECT * FROM ReviewsAccommodations WHERE AccommodationId = @AccommodationId";
                    using (SqlCommand command = new SqlCommand(queryToDisplayReview, conn))
                    {
                        command.Parameters.AddWithValue("@AccommodationId", accommodationId);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ReviewAccommodation review = new ReviewAccommodation
                                {
                                    Rating = (int)reader["Rating"],
                                    Comment = reader["Comment"].ToString(),
                                    CreatedAt = (DateTime)reader["CreatedAt"]
                                };
                                reviewAccommodations.Add(review);
                            }
                        }
                    }
                }
                return View(reviewAccommodations);
            }
            catch (Exception e)
            {
                return View("Error" + e);
            }
        }


     
    }
}