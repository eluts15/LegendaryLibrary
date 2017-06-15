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

    public Book(string name, string genre, int id = 0)
    {
      _name = name;
      _genre = genre;
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
        return (idEquality && nameEquality && genreEquality);
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
        Book newBook = new Book(name, genre, id);
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

      SqlCommand cmd = new SqlCommand("INSERT INTO books (name, genre) OUTPUT INSERTED.id VALUES (@name, @genre);", conn);

      SqlParameter namePara = new SqlParameter("@name", this.GetName());
      SqlParameter genrePara = new SqlParameter("@genre", this.GetGenre());

      cmd.Parameters.Add(namePara);
      cmd.Parameters.Add(genrePara);

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

      while(rdr.Read())
      {
        foundId = rdr.GetInt32(0);
        name = rdr.GetString(1);
        genre = rdr.GetString(2);
      }
      Book foundBook = new Book(name, genre, foundId);
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

    public void Update(string name, string genre)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE books SET name = @name, genre = @genre WHERE id = @Id;", conn);

      SqlParameter namePara = new SqlParameter("@name", name);
      SqlParameter genrePara = new SqlParameter("@genre", genre);
      SqlParameter idPara = new SqlParameter("@Id", this.GetId());

      cmd.Parameters.Add(namePara);
      cmd.Parameters.Add(genrePara);
      cmd.Parameters.Add(idPara);

      this._name = name;
      this._genre = genre;
      cmd.ExecuteNonQuery();
      conn.Close();
    }


    public List<Author> GetAuthors()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT authors.* FROM books JOIN books_authors ON (books.id = books_authors.book_id) JOIN authors ON (books_authors.author_id = authors.id) WHERE books.id = @BooksId;", conn);
      SqlParameter BooksIdParam = new SqlParameter("@BooksId", this.GetId().ToString());

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
