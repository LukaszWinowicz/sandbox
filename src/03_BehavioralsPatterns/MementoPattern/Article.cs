using System;
using System.Collections.Generic;
using System.Text;

namespace MementoPattern;

public class Article
{
    public string Content { get; set; }
    public string Title { get; set; }
    public byte[] Attachment { get; set; }

    // Backup
    public ArticleMemento CreateSnapshot() => new ArticleMemento(Content, Title);
    
    // Restore
    public void SetSnapshot(ArticleMemento snapshot)
    {
        this.Content = snapshot.Content;
        this.Title = snapshot.Title;
    }

}

// Migawka (snapshot)
public class ArticleMemento
{
    public string Content { get;  }
    public string Title { get; }
    public DateTime SnapshotDate { get; }

    public ArticleMemento(string content, string title)
    {
        Content = content;
        Title = title;
        SnapshotDate = DateTime.UtcNow;
    }

    public override string ToString()
    {
        return $"{SnapshotDate}: {Title} {Content}";
    }
}

// Abstract Caretaker (Nadzorca)
public interface IArticleCaretaker
{
    void SetState(ArticleMemento memento);
    ArticleMemento GetState();
}

public interface IDateArticleCaretaker 
{
    void SetState(ArticleMemento memento);
    ArticleMemento GetState(DateTime date);
}

// Concrete Caretaker A
public class StackArticleCaretaker : IArticleCaretaker
{
    private readonly Stack<ArticleMemento> snapshots = new Stack<ArticleMemento>();

    public ArticleMemento GetState() => snapshots.Pop();

    public void SetState(ArticleMemento memento) => snapshots.Push(memento);
}



public class HistoryArticleCaretaker : IDateArticleCaretaker
{
    private readonly Dictionary<DateTime, ArticleMemento> snapshots = new Dictionary<DateTime, ArticleMemento>();

    public ArticleMemento GetState(DateTime snapshotDate) => snapshots[snapshotDate];

    public void SetState(ArticleMemento memento) => snapshots.Add(DateTime.Now, memento);
}

