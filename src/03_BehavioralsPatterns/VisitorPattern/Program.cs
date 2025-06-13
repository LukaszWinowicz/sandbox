using System;
using System.Collections.ObjectModel;

namespace VisitorPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Visitor Pattern!");

            Form form = Get();

            IVisitor visitor = new HtmlVisitor();

            form.Accept(visitor);

            string html = visitor.GetResult();

            System.IO.File.WriteAllText("index.html", html);
        }

        public static Form Get()
        {
            Form form = new Form
            {
                Name = "/forms/customers",
                Title = "Design Patterns",

                Body = new Collection<Control>
                {

                    new Label { Caption = "Person", Name = "lblName" },
                    new TextBox { Caption = "FirstName", Name = "txtFirstName", Value = "John"},
                    new Checkbox { Caption = "IsAdult", Name = "chkIsAdult", Value = true },
                    new Button {  Caption = "Submit", Name = "btnSubmit", ImageSource = "save.png" },
                }

            };

            return form;
        }
    }

}
