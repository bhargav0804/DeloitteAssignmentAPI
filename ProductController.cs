using DeliotteWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DeliotteWebAPI.Controllers
{
   // [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProductController : ApiController
    {
        ProductEntities productEntities = new ProductEntities();

       //[EnableCors("http://localhost:8080/", "*", "*")]
        //GET: api/product
        public HttpResponseMessage Get()
        {
            try
            {
                var products = productEntities.Products.ToList();

                if (products.Any())
                {
                    // If products are found
                    var responseData = new
                    {
                        response = products,
                        status = (int)HttpStatusCode.OK
                };

                 //   System.Web.HttpContext.Current.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
                    // Return a 200 OK response 
                    return Request.CreateResponse(HttpStatusCode.OK, responseData);
                }
                else
                {
                    // If no products are found
                    var responseData = new
                    {
                        response = new List<object>(),
                        status = (int)HttpStatusCode.NotFound
                    };

                  //  System.Web.HttpContext.Current.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
                    // Return a 404 Not Found response 
                    return Request.CreateResponse(HttpStatusCode.NotFound, responseData);
                }
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    response = new List<object>(), // Empty array
                    status = (int)HttpStatusCode.InternalServerError,
                    errorMessage = ex.Message // Include the error message if needed
                };

                return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            }

        }

        //GET: api/product/id
        public HttpResponseMessage GetProductId(int id)
        {
            try
            {
                Product product = productEntities.Products.Find(id);

                if (product != null)
                {
                    var responseData = new
                    {
                        response = product,
                        status = (int)HttpStatusCode.OK
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, responseData);
                }
                else
                {
                    var responseData = new
                    {
                        response = new { }, // Empty object
                        status = (int)HttpStatusCode.NotFound
                    };

                    return Request.CreateResponse(HttpStatusCode.NotFound, responseData);
                }
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    response = new { }, // Empty object
                    status = (int)HttpStatusCode.InternalServerError,
                    errorMessage = ex.Message // Include the error message if needed
                };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            }

        }

        // POST: api/product
        public HttpResponseMessage Post(Product product)
        {
            try
            {
                productEntities.Products.Add(product);
                productEntities.SaveChanges();

                var responseData = new
                {
                    response = "Product Added Successfully",
                    status = (int)HttpStatusCode.Created // Assuming successful creation
                };

                return Request.CreateResponse(HttpStatusCode.Created, responseData);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    response = new { }, // Empty object
                    status = (int)HttpStatusCode.InternalServerError,
                    errorMessage = ex.Message // Include the error message if needed
                };

                return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            }

        }

        //PUT: api/product/80
        public HttpResponseMessage Put(int id, Product product)
        {
            try
            {
                var productFind = productEntities.Products.Find(id);

                if (productFind != null)
                {
                    productFind.ProductName = product.ProductName;
                    productFind.SupplierID = product.SupplierID;
                    productFind.CategoryID = product.CategoryID;
                    productFind.QuantityPerUnit = product.QuantityPerUnit;
                    productFind.UnitPrice = product.UnitPrice;
                    productFind.UnitsInStock = product.UnitsInStock;
                    productFind.UnitsOnOrder = product.UnitsOnOrder;
                    productFind.ReorderLevel = product.ReorderLevel;
                    productFind.Discontinued = product.Discontinued;

                    productEntities.Entry(productFind).State = EntityState.Modified;
                    productEntities.SaveChanges();

                    var responseData = new
                    {
                        response = new List<string> { "Product edited" }, // Wrap the message in a list to match the desired format
                        status = (int)HttpStatusCode.OK // Assuming successful update
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, responseData);
                }
                else
                {
                    var responseData = new
                    {
                        response = new List<object>(), // Empty array
                        status = (int)HttpStatusCode.NotFound
                    };

                    return Request.CreateResponse(HttpStatusCode.NotFound, responseData);
                }
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    response = new List<object>(), // Empty array
                    status = (int)HttpStatusCode.InternalServerError,
                    errorMessage = ex.Message // Include the error message if needed
                };

                return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            }

        }

        //DELETE: api/product/80
        public HttpResponseMessage Delete(int Id)
        {
            try
            {
                Product product = productEntities.Products.Find(Id);

                if (product != null)
                {
                    productEntities.Products.Remove(product);
                    productEntities.SaveChanges();

                    var responseData = new
                    {
                        response = new List<string> { "Product Deleted" }, // Wrap the message in a list to match the desired format
                        status = (int)HttpStatusCode.OK // Assuming successful deletion
                    };
                    //Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    return Request.CreateResponse(HttpStatusCode.OK, responseData);
                }
                else
                {
                    var responseData = new
                    {
                        response = new List<object>(), // Empty array
                        status = (int)HttpStatusCode.NotFound
                    };

                    return Request.CreateResponse(HttpStatusCode.NotFound, responseData);
                }
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    response = new List<object>(), // Empty array
                    status = (int)HttpStatusCode.InternalServerError,
                    errorMessage = ex.Message // Include the error message if needed
                };

                return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            }

        }

        // [EnableCors("*", "*", "*")]
        //POST: api/Product/searchresult
        [HttpPost]
        [Route("api/product/searchresult")]
        public HttpResponseMessage SearchProductsByName(SearchRequest searchRequest)
        {
            #region
            //try
            //{
            //    if (searchRequest != null && !string.IsNullOrEmpty(searchRequest.ProductName))
            //    {
            //        var matchingProducts = productEntities.Products
            //            .Where(p => p.ProductName.Contains(searchRequest.ProductName))
            //            .ToList();

            //        // Create a response with the desired structure
            //        var responseData = new
            //        {
            //            response = matchingProducts,
            //            status = (int)HttpStatusCode.OK
            //        };

            //     //   System.Web.HttpContext.Current.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
            //        // Return a 200 OK response with the structured data
            //        return Request.CreateResponse(HttpStatusCode.OK, responseData);
            //    }
            //    else
            //    {
            //        // If the search request is invalid or the product name is empty, create a response with the desired structure
            //        var responseData = new
            //        {
            //            response = new List<object>(), // Empty array
            //            status = (int)HttpStatusCode.BadRequest
            //        };

            //     //   System.Web.HttpContext.Current.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
            //        // Return a 400 Bad Request response with the structured data
            //        return Request.CreateResponse(HttpStatusCode.BadRequest, responseData);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    // Handle exceptions and return a 500 Internal Server Error response
            //    var errorResponse = new
            //    {
            //        response = new List<object>(), // Empty array
            //        status = (int)HttpStatusCode.InternalServerError,
            //        errorMessage = ex.Message // Include the error message if needed
            //    };

            //  //  System.Web.HttpContext.Current.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
            //    return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            //}
            #endregion

            try
            {
                var matchingProducts = productEntities.Products.AsQueryable();

                if (searchRequest != null)
                {
                    if (searchRequest.ProductID > 0)
                    {
                        matchingProducts = matchingProducts.Where(p => p.ProductID == searchRequest.ProductID);
                    }

                    if (!string.IsNullOrEmpty(searchRequest.ProductName))
                    {
                        matchingProducts = matchingProducts.Where(p => p.ProductName.Contains(searchRequest.ProductName));
                    }

                    if (searchRequest.SupplierID > 0)
                    {
                        matchingProducts = matchingProducts.Where(p => p.SupplierID == searchRequest.SupplierID);
                    }

                    if (searchRequest.CategoryID > 0)
                    {
                        matchingProducts = matchingProducts.Where(p => p.CategoryID == searchRequest.CategoryID);
                    }

                    if (!string.IsNullOrEmpty(searchRequest.QuantityPerUnit))
                    {
                        matchingProducts = matchingProducts.Where(p => p.QuantityPerUnit.Contains(searchRequest.QuantityPerUnit));
                    }

                    if (searchRequest.UnitPrice > 0)
                    {
                        matchingProducts = matchingProducts.Where(p => p.UnitPrice == searchRequest.UnitPrice);
                    }

                    if (searchRequest.UnitsInStock > 0)
                    {
                        matchingProducts = matchingProducts.Where(p => p.UnitsInStock == searchRequest.UnitsInStock);
                    }

                    if (searchRequest.UnitsOnOrder > 0)
                    {
                        matchingProducts = matchingProducts.Where(p => p.UnitsOnOrder == searchRequest.UnitsOnOrder);
                    }

                    if (searchRequest.ReorderLevel > 0)
                    {
                        matchingProducts = matchingProducts.Where(p => p.ReorderLevel == searchRequest.ReorderLevel);
                    }

                    matchingProducts = matchingProducts.Where(p => p.Discontinued == searchRequest.Discontinued);
                }

                var responseData = new
                {
                    response = matchingProducts.ToList(),
                    status = (int)HttpStatusCode.OK
                };

                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    response = new List<object>(), // Empty array
                    status = (int)HttpStatusCode.InternalServerError,
                    errorMessage = ex.Message // Include the error message if needed
                };

                return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            }
        }
    }
}
