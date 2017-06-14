using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Library
{
  public class Book
  {
    private int _id;
    private string _name;
    private string _genre;
    private DateTime _dueDate;

    public Book(string name, string genre, DateTime dueDate, int id = 0)
    {
      _name = name;
      _genre = genre;
      _dueDate = dueDate;
      _id = id;
    }

    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public string GetGenre()
    {
      return _genre;
    }
    public DateTime GetDueDate()
    {
      return _dueDate;
    }

    public override bool Equals(System.Object otherBook)
    {
      if(!(otherBook is Book))
      {
        return false;
      }
      else
      {
        Book newBook = (Book) otherBook;
        bool idEquality = (this.GetId() == newBook.GetId());
        bool nameEquality = (this.GetName() == newBook.GetName());
        bool genreEquality = (this.GetGenre() == newBook.GetGenre());
        bool dueDateEquality = (this.GetDueDate() == newBook.GetDueDate());
        return (idEquality && nameEquality && genreEquality && dueDateEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.GetName().GetHashCode();
    }

    public static List<Book> GetAll()
    {
      List<Book> AllBook = new List<Book>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM books", conn);
      SqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        string genre = rdr.GetString(2);
        DateTime dueDate = rdr.GetDateTime(3);
        Book newBook = new Book(name, genre, dueDate, id);
        AllBook.Add(newBook);
      }
      if (rdr != null)
      {
       rdr.Close();
      }
      if (conn != null)
      {
       conn.Close();
      }
      return AllBook;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO books (name, genre, due_date) OUTPUT INSERTED.id VALUES (@name, @genre, @dueDate);", conn);

      SqlParameter namePara = new SqlParameter("@name", this.GetName());
      SqlParameter genrePara = new SqlParameter("@genre", this.GetGenre());
      SqlParameter dueDate = new SqlParameter("@dueDate", this.GetDueDate());

      cmd.Parameters.Add(namePara);
      cmd.Parameters.Add(genrePara);
      cmd.Parameters.Add(dueDate);

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }

    public static Book Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM books WHERE id = @id;", conn);
      SqlParameter idParameter = new SqlParameter("@id", id.ToString());

      cmd.Parameters.Add(idParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundId = 0;
      string name = null;
      string genre = null;
      DateTime dueDate = new DateTime();

      while(rdr.Read())
      {
        foundId = rdr.GetInt32(0);
        name = rdr.GetString(1);
        genre = rdr.GetString(2);
        dueDate = rdr.GetDateTime(3);
      }
      Book foundBook = new Book(name, genre, dueDate, foundId);
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundBook;
    }

    public void Update(string name, string genre, DateTime dueDate)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE books SET name = @name, due_date = @dueDate, genre = @genre WHERE id = @Id;", conn);

      SqlParameter namePara = new SqlParameter("@name", name);
      SqlParameter genrePara = new SqlParameter("@genre", genre);
      SqlParameter dueDatePara = new SqlParameter("@dueDate", dueDate);
      SqlParameter idPara = new SqlParameter("@Id", this.GetId());

      cmd.Parameters.Add(namePara);
      cmd.Parameters.Add(genrePara);
      cmd.Parameters.Add(dueDatePara);
      cmd.Parameters.Add(idPara);

      this._name = name;
      this._genre = genre;
      this._dueDate = dueDate;
      cmd.ExecuteNonQuery();
      conn.Close();
    }


    public List<Author> GetAuthors()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT authors.* FROM books JOIN books_authors ON (books.id = books_authors.book_id) JOIN authors ON (books_authors.author_id = authors.id) WHERE books.id = @BooksId;", conn);
      SqlParameter BooksIdParam = new SqlParameter();
      BooksIdParam.ParameterName = "@BooksId";
      BooksIdParam.Value = this.GetId().ToString();

      cmd.Parameters.Add(BooksIdParam);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Author> authors = new List<Author>{};

      while(rdr.Read())
      {
        int authorId = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        Author newAuthor = new Author(name, authorId);
        authors.Add(newAuthor);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return authors;
    }
    //Add book's id and author's id to books_authors table
    public void AddAuthor(Author newAuthor)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO books_authors (book_id, author_id) VALUES (@BookId, @AuthorId);", conn);

      SqlParameter bookIdParameter = new SqlParameter("@BookId", this.GetId());
      SqlParameter authorIdParameter = new SqlParameter( "@AuthorId", newAuthor.GetId());

      cmd.Parameters.Add(bookIdParameter);
      cmd.Parameters.Add(authorIdParameter);
      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM books WHERE id = @bookId; DELETE FROM books_authors WHERE book_id = @bookId;", conn);
      SqlParameter bookIdParameter = new SqlParameter("@bookId", this.GetId());

      cmd.Parameters.Add(bookIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
       conn.Close();
      }
    }

    public static void DeleteAll()
    {
     SqlConnection conn = DB.Connection();
     conn.Open();
     SqlCommand cmd = new SqlCommand("DELETE FROM books;", conn);
     cmd.ExecuteNonQuery();
     conn.Close();
    }
  }
}
