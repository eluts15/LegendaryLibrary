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
    private int _copies;

    public Book(string name, string genre, int copies, int id = 0)
    {
      _name = name;
      _genre = genre;
      _copies = copies;
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
    public int GetCopies()
    {
      return _copies;
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
        bool copiesEquality = (this.GetCopies() == newBook.GetCopies());
        return (idEquality && nameEquality && genreEquality && copiesEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.GetName().GetHashCode();
    }

    public static List<Book> GetAll()
    {
      List<Book> AllBooks = new List<Book>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM books", conn);
      SqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        string genre = rdr.GetString(2);
        int copies = rdr.GetInt32(3);
        Book newBook = new Book(name, genre, copies, id);
        AllBooks.Add(newBook);
      }
      if (rdr != null)
      {
       rdr.Close();
      }
      if (conn != null)
      {
       conn.Close();
      }
      return AllBooks;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO books (name, genre, copies) OUTPUT INSERTED.id VALUES (@name, @genre, @copies);", conn);

      SqlParameter nameParam = new SqlParameter("@name", this.GetName());
      SqlParameter genreParam = new SqlParameter("@genre", this.GetGenre());
      SqlParameter copiesParam = new SqlParameter("@copies", this.GetCopies());

      cmd.Parameters.Add(nameParam);
      cmd.Parameters.Add(genreParam);
      cmd.Parameters.Add(copiesParam);

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
      SqlParameter idParam = new SqlParameter("@id", id.ToString());

      cmd.Parameters.Add(idParam);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundId = 0;
      string name = null;
      string genre = null;
      int copies = 0;

      while(rdr.Read())
      {
        foundId = rdr.GetInt32(0);
        name = rdr.GetString(1);
        genre = rdr.GetString(2);
        copies = rdr.GetInt32(3);
      }
      Book foundBook = new Book(name, genre, copies, foundId);
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

    public void Update(string name, string genre, int copies)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE books SET name = @name, genre = @genre, copies = @copies WHERE id = @Id;", conn);

      SqlParameter nameParam = new SqlParameter("@name", name);
      SqlParameter genreParam = new SqlParameter("@genre", genre);
      SqlParameter copiesParam = new SqlParameter("@copies", copies);
      SqlParameter idParam = new SqlParameter("@Id", this.GetId());

      cmd.Parameters.Add(nameParam);
      cmd.Parameters.Add(genreParam);
      cmd.Parameters.Add(copiesParam);
      cmd.Parameters.Add(idParam);

      this._name = name;
      this._genre = genre;
      this._copies = copies;
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

      SqlParameter bookIdParam = new SqlParameter("@BookId", this.GetId());
      SqlParameter authorIdParam = new SqlParameter( "@AuthorId", newAuthor.GetId());

      cmd.Parameters.Add(bookIdParam);
      cmd.Parameters.Add(authorIdParam);
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
      SqlParameter bookIdParam = new SqlParameter("@bookId", this.GetId());

      cmd.Parameters.Add(bookIdParam);
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
