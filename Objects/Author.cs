using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Library
{
  public class Author
  {
    private int _id;
    private string _name;

    public Author (string name, int id = 0)
    {
      _name = name;
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

    public override bool Equals(System.Object otherAuthor)
    {
     if(!(otherAuthor is Author))
     {
       return false;
     }
     else
      {
       Author newAuthor = (Author) otherAuthor;
       bool idEquality = (this.GetId() == newAuthor.GetId());
       bool nameEquality = (this.GetName() == newAuthor.GetName());
       return (idEquality && nameEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.GetName().GetHashCode();
    }

    public static List<Author> GetAll()
    {
      List<Author> AllAuthor = new List<Author>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM authors;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        Author newAuthor = new Author(name, id);
        AllAuthor.Add(newAuthor);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return AllAuthor;
    }

    public void Save()
    {
     SqlConnection conn = DB.Connection();
     conn.Open();

     SqlCommand cmd = new SqlCommand("INSERT INTO authors (name) OUTPUT INSERTED.id VALUES (@name);", conn);

     SqlParameter namePara = new SqlParameter("@name", this.GetName());

     cmd.Parameters.Add(namePara);
     SqlDataReader rdr = cmd.ExecuteReader();

     while(rdr.Read())
     {
       this._id = rdr.GetInt32(0);
     }
     if (rdr != null)
     {
       rdr.Close();
     }
     if (conn != null)
     {
       conn.Close();
     }
    }

    public static Author Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM authors WHERE id = @id;", conn);
      SqlParameter IdPara = new SqlParameter("@id", id.ToString());

      cmd.Parameters.Add(IdPara);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundId = 0;
      string name = null;

      while(rdr.Read())
      {
        foundId = rdr.GetInt32(0);
        name = rdr.GetString(1);
      }
      Author foundAuthor = new Author(name, foundId);
      if (rdr != null)
      {
       rdr.Close();
      }
      if (conn != null)
      {
       conn.Close();
      }
     return foundAuthor;
    }

    // public void Update(string name, string author_number, string completion, string grade)
    // {
    // SqlConnection conn = DB.Connection();
    // conn.Open();
    //
    // SqlCommand cmd = new SqlCommand("UPDATE authors SET name = @name, author_number = @author_number, completion = @completion, grade = @grade WHERE id = @Id;", conn);
    //
    // SqlParameter namePara = new SqlParameter("@name", name);
    // SqlParameter authorPara = new SqlParameter("@author_number", author_number);
    // SqlParameter completionPara = new SqlParameter("@completion", completion);
    // SqlParameter gradePara = new SqlParameter("@grade", grade);
    // SqlParameter idPara = new SqlParameter("@Id", this.GetId());
    //
    // cmd.Parameters.Add(namePara);
    // cmd.Parameters.Add(authorPara);
    // cmd.Parameters.Add(completionPara);
    // cmd.Parameters.Add(gradePara);
    // cmd.Parameters.Add(idPara);
    //
    // this._name = name;
    // this._author_number = author_number;
    // this._completion = completion;
    // this._grade = grade;
    // cmd.ExecuteNonQuery();
    // conn.Close();
    // }
    // //Add book's id and author's id to authors_books table
    // public void AddBook(Book newBook)
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("INSERT INTO authors_books (authors_id, books_id) VALUES (@AuthorId, @BookId);", conn);
    //
    //   SqlParameter authorIdParameter = new SqlParameter("@AuthorId", this.GetId());
    //   SqlParameter bookIdParameter = new SqlParameter( "@BookId", newBook.GetId());
    //
    //   cmd.Parameters.Add(authorIdParameter);
    //   cmd.Parameters.Add(bookIdParameter);
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
    //   SqlCommand cmd = new SqlCommand("DELETE FROM authors WHERE id = @Id; DELETE FROM authors_books WHERE authors_id = @Id;", conn);
    //   SqlParameter IdParameter = new SqlParameter("@Id", this.GetId());
    //
    //   cmd.Parameters.Add(IdParameter);
    //   cmd.ExecuteNonQuery();
    //
    //   if (conn != null)
    //   {
    //     conn.Close();
    //   }
    // }
    //
    // public List<Book> GetBook()
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("SELECT books.* FROM authors JOIN authors_books ON (authors.id = authors_books.authors_id) JOIN books ON (authors_books.books_id = books.id) WHERE authors.id = @authorId;", conn);
    //   SqlParameter AuthorIdParam = new SqlParameter("@authorId", this.GetId().ToString());
    //
    //   cmd.Parameters.Add(AuthorIdParam);
    //
    //   SqlDataReader rdr = cmd.ExecuteReader();
    //
    //   List<Book> books = new List<Book>{};
    //
    //   while(rdr.Read())
    //   {
    //     int bookId = rdr.GetInt32(0);
    //     string name = rdr.GetString(1);
    //     DateTime enrollment = rdr.GetDateTime(2);
    //     string major = rdr.GetString(3);
    //   Book newBook = new Book(name, enrollment, major, bookId);
    //     books.Add(newBook);
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
    //   return books;
    // }
    //
    //
    //

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM authors;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
