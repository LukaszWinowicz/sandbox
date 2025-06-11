using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuilderPattern.Models
{
    public class SalesReport
    {
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public decimal TotalSalesAmount { get; set; }

        public IEnumerable<ProductReportDetail> ProductDetails { get; set; } = new List<ProductReportDetail>();
        public IEnumerable<GenderReportDetail> GenderDetails { get; set; } = new List<GenderReportDetail>();


        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("------------------------------");

            builder.AppendLine($"{Title} {CreateDate}");
            builder.AppendLine($"Total Sales Amount: {TotalSalesAmount:c2}");

            builder.AppendLine("------------------------------");

            if (ProductDetails.Any())
            {
                builder.AppendLine("Total By Products:");
                foreach (var detail in ProductDetails)
                {
                    builder.AppendLine($"- {detail.Product.Name} {detail.Quantity} {detail.TotalAmount:c2}");
                }
            }

            if (GenderDetails.Any())
            {
                builder.AppendLine("Total By Gender:");
                foreach (var detail in GenderDetails)
                {
                    builder.AppendLine($"- {detail.Gender} {detail.Quantity} {detail.TotalAmount:c2}");
                }
            }

            return builder.ToString();
        }
    }




}