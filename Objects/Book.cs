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

    public Book(string name, DateTime dueDate, string genre, int id = 0)
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
        Book newBook = new Book(name, dueDate, genre, id);
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

    // public void Save()
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("INSERT INTO books (name, dueDate, genre) OUTPUT INSERTED.id VALUES (@name, @dueDate, @genre);", conn);
    //
    //   SqlParameter namePara = new SqlParameter("@name", this.GetName());
    //   SqlParameter dueDate = new SqlParameter("@dueDate", this.GetDueDate());
    //   SqlParameter genrePara = new SqlParameter("@genre", this.GetGenre());
    //
    //   cmd.Parameters.Add(namePara);
    //   cmd.Parameters.Add(dueDate);
    //   cmd.Parameters.Add(genrePara);
    //
    //   SqlDataReader rdr = cmd.ExecuteReader();
    //
    //   while(rdr.Read())
    //   {
    //     this._id = rdr.GetInt32(0);
    //   }
    //   if(rdr != null)
    //   {
    //     rdr.Close();
    //   }
    //   if(conn != null)
    //   {
    //     conn.Close();
    //   }
    // }
    //
    // public static Book Find(int id)
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("SELECT * FROM books WHERE id = @id;", conn);
    //   SqlParameter idParameter = new SqlParameter("@id", id.ToString());
    //
    //   cmd.Parameters.Add(idParameter);
    //   SqlDataReader rdr = cmd.ExecuteReader();
    //
    //   int foundId = 0;
    //   string name = null;
    //   DateTime dueDate = new DateTime();
    //   string genre = null;
    //
    //   while(rdr.Read())
    //   {
    //     foundId = rdr.GetInt32(0);
    //     name = rdr.GetString(1);
    //     dueDate = rdr.GetDateTime(2);
    //     genre = rdr.GetString(3);
    //   }
    //   Book foundBook = new Book(name, dueDate, genre, foundId);
    //   if (rdr != null)
    //   {
    //     rdr.Close();
    //   }
    //   if (conn != null)
    //   {
    //     conn.Close();
    //   }
    //   return foundBook;
    // }
    //
    // public void Update(string name, DateTime dueDate, string genre)
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("UPDATE books SET name = @name, dueDate = @dueDate, genre = @genre WHERE id = @Id;", conn);
    //
    //   SqlParameter namePara = new SqlParameter("@name", name);
    //   SqlParameter dueDatePara = new SqlParameter("@dueDate", dueDate);
    //   SqlParameter genrePara = new SqlParameter("@genre", genre);
    //   SqlParameter idPara = new SqlParameter("@Id", this.GetId());
    //
    //   cmd.Parameters.Add(namePara);
    //   cmd.Parameters.Add(dueDatePara);
    //   cmd.Parameters.Add(genrePara);
    //   cmd.Parameters.Add(idPara);
    //
    //   this._name = name;
    //   this._dueDate = dueDate;
    //   this._genre = genre;
    //   cmd.ExecuteNonQuery();
    //   conn.Close();
    // }
    //
    //
    // public List<Author> GetAuthors()
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("SELECT authors.* FROM books JOIN authors_books ON (books.id = authors_books.books_id) JOIN authors ON (authors_books.authors_id = authors.id) WHERE books.id = @BooksId;", conn);
    //   SqlParameter BooksIdParam = new SqlParameter();
    //   BooksIdParam.ParameterName = "@BooksId";
    //   BooksIdParam.Value = this.GetId().ToString();
    //
    //   cmd.Parameters.Add(BooksIdParam);
    //
    //   SqlDataReader rdr = cmd.ExecuteReader();
    //
    //   List<Author> authors = new List<Author>{};
    //
    //   while(rdr.Read())
    //   {
    //     int authorId = rdr.GetInt32(0);
    //     string name = rdr.GetString(1);
    //     string author_number = rdr.GetString(2);
    //     string completion = rdr.GetString(3);
    //     string grade = rdr.GetString(4);
    //     Author newAuthor = new Author(name, author_number, completion, grade, authorId);
    //     authors.Add(newAuthor);
    //   }
    //
    //   if (rdr != null)
    //   {
    //     rdr.Close();
    //   }
    //   if (conn != null)
    //   {
    //     conn.Close();
    //   }
    //   return authors;
    // }
    //
    // public void AddAuthor(Author newAuthor)
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("INSERT INTO authors_books (authors_id, books_id) VALUES (@AuthorId, @BookId);", conn);
    //
    //   SqlParameter bookIdParameter = new SqlParameter("@BookId", this.GetId());
    //   SqlParameter authorIdParameter = new SqlParameter( "@AuthorId", newAuthor.GetId());
    //
    //   cmd.Parameters.Add(bookIdParameter);
    //   cmd.Parameters.Add(authorIdParameter);
    //   cmd.ExecuteNonQuery();
    //   if (conn != null)
    //   {
    //     conn.Close();
    //   }
    // }
    //
    // public void Delete()
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("DELETE FROM books WHERE id = @bookId; DELETE FROM authors_books WHERE books_id = @bookId;", conn);
    //   SqlParameter bookIdParameter = new SqlParameter("@bookId", this.GetId());
    //
    //   cmd.Parameters.Add(bookIdParameter);
    //   cmd.ExecuteNonQuery();
    //
    //   if (conn != null)
    //   {
    //    conn.Close();
    //   }
    // }

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
