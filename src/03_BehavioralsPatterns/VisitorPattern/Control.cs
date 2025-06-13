using System.ComponentModel;
using System.Text;

namespace VisitorPattern;


// Abstract Element
public abstract class Control
{
    public string Name { get; set; }
    public string Caption { get; set; }

    public abstract void Accept(IVisitor visitor);
}

// Concrete Element
public class Label : Control
{
    public override void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class TextBox : Control
{
    public string Value { get; set; }

    public override void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class Checkbox : Control
{
    public bool Value { get; set; }

    public override void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class Button : Control
{
    public string ImageSource { get; set; }

    public override void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}


// Abstract Visitor
public interface IVisitor
{
    void Visit(Label label);
    void Visit(TextBox textBox);
    void Visit(Checkbox checkbox);
    void Visit(Button button);

    string GetResult();
}


public class HtmlVisitor : IVisitor
{
    private StringBuilder builder = new StringBuilder();

    public HtmlVisitor()
    {
        AddHeader();
    }

    private void AddHeader()
    {
        builder.AppendLine("<html>");
    }

    

    public void Visit(Label control)
    {
        builder.AppendLine($"<span>{control.Caption}</span>");
    }

    public void Visit(TextBox control)
    {
        builder.AppendLine($"<span>{control.Caption}</span><input type='text' value='{control.Value}'></input>");
    }

    public void Visit(Checkbox control)
    {
        builder.AppendLine($"<span>{control.Caption}</span><input type='checkbox' value='{control.Value}'></input>");
    }

    public void Visit(Button control)
    {
        builder.AppendLine($"<button><img src='{control.ImageSource}'/>{control.Caption}</button>");
    }

    private void AddFooter()
    {
        builder.AppendLine("</html>");
    }


    public string GetResult()
    {
        AddFooter();

        return builder.ToString();
    }
}