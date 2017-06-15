using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Library
{
  // public class Patron
  // {
  //   private int _id;
  //   private string _name;
  //
  //   public Patron (string name, int id = 0)
  //   {
  //     _name = name;
  //     _id = id;
  //   }
  //   public int GetId()
  //   {
  //     return _id;
  //   }
  //   public string GetName()
  //   {
  //     return _name;
  //   }
  //
  //   public override bool Equals(System.Object otherPatron)
  //   {
  //    if(!(otherPatron is Patron))
  //    {
  //      return false;
  //    }
  //    else
  //     {
  //      Patron newPatron = (Patron) otherPatron;
  //      bool idEquality = (this.GetId() == newPatron.GetId());
  //      bool nameEquality = (this.GetName() == newPatron.GetName());
  //      return (idEquality && nameEquality);
  //     }
  //   }
  //
  //   public override int GetHashCode()
  //   {
  //     return this.GetName().GetHashCode();
  //   }
  //
  //   public static List<Patron> GetAll()
  //   {
  //     List<Patron> AllPatron = new List<Patron>{};
  //
  //     SqlConnection conn = DB.Connection();
  //     conn.Open();
  //
  //     SqlCommand cmd = new SqlCommand("SELECT * FROM Patrons;", conn);
  //     SqlDataReader rdr = cmd.ExecuteReader();
  //
  //     while(rdr.Read())
  //     {
  //       int id = rdr.GetInt32(0);
  //       string name = rdr.GetString(1);
  //       Patron newPatron = new Patron(name, id);
  //       AllPatron.Add(newPatron);
  //     }
  //     if (rdr != null)
  //     {
  //       rdr.Close();
  //     }
  //     if (conn != null)
  //     {
  //       conn.Close();
  //     }
  //     return AllPatron;
  //   }
  //
  //   public List<Book> GetBooks()
  //   {
  //     SqlConnection conn = DB.Connection();
  //     conn.Open();
  //
  //     SqlCommand cmd = new SqlCommand("SELECT books.* FROM Patrons JOIN books_Patrons ON (Patrons.id = books_Patrons.Patron_id) JOIN books ON (books_Patrons.book_id = books.id) WHERE Patrons.id = @PatronId;", conn);
  //     SqlParameter PatronIdParam = new SqlParameter("@PatronId", this.GetId().ToString());
  //
  //     cmd.Parameters.Add(PatronIdParam);
  //
  //     SqlDataReader rdr = cmd.ExecuteReader();
  //
  //     List<Book> books = new List<Book>{};
  //
  //     while(rdr.Read())
  //     {
  //       int bookId = rdr.GetInt32(0);
  //       string name = rdr.GetString(1);
  //       string genre = rdr.GetString(2);
  //       DateTime dueDate = rdr.GetDateTime(3);
  //       Book newBook = new Book(name, genre, dueDate, bookId);
  //       books.Add(newBook);
  //     }
  //
  //     if (rdr != null)
  //     {
  //       rdr.Close();
  //     }
  //     if (conn != null)
  //     {
  //       conn.Close();
  //     }
  //     return books;
  //   }
  //
  //   public void Save()
  //   {
  //    SqlConnection conn = DB.Connection();
  //    conn.Open();
  //
  //    SqlCommand cmd = new SqlCommand("INSERT INTO Patrons (name) OUTPUT INSERTED.id VALUES (@name);", conn);
  //
  //    SqlParameter namePara = new SqlParameter("@name", this.GetName());
  //
  //    cmd.Parameters.Add(namePara);
  //    SqlDataReader rdr = cmd.ExecuteReader();
  //
  //    while(rdr.Read())
  //    {
  //      this._id = rdr.GetInt32(0);
  //    }
  //    if (rdr != null)
  //    {
  //      rdr.Close();
  //    }
  //    if (conn != null)
  //    {
  //      conn.Close();
  //    }
  //   }
  //
  //   public static Patron Find(int id)
  //   {
  //     SqlConnection conn = DB.Connection();
  //     conn.Open();
  //
  //     SqlCommand cmd = new SqlCommand("SELECT * FROM Patrons WHERE id = @id;", conn);
  //     SqlParameter IdPara = new SqlParameter("@id", id.ToString());
  //
  //     cmd.Parameters.Add(IdPara);
  //     SqlDataReader rdr = cmd.ExecuteReader();
  //
  //     int foundId = 0;
  //     string name = null;
  //
  //     while(rdr.Read())
  //     {
  //       foundId = rdr.GetInt32(0);
  //       name = rdr.GetString(1);
  //     }
  //     Patron foundPatron = new Patron(name, foundId);
  //     if (rdr != null)
  //     {
  //      rdr.Close();
  //     }
  //     if (conn != null)
  //     {
  //      conn.Close();
  //     }
  //    return foundPatron;
  //   }
  //
  //   public void Update(string name)
  //   {
  //     SqlConnection conn = DB.Connection();
  //     conn.Open();
  //
  //     SqlCommand cmd = new SqlCommand("UPDATE Patrons SET name = @name WHERE id = @Id;", conn);
  //
  //     SqlParameter namePara = new SqlParameter("@name", name);
  //     SqlParameter idPara = new SqlParameter("@Id", this.GetId());
  //
  //     cmd.Parameters.Add(namePara);
  //     cmd.Parameters.Add(idPara);
  //
  //     this._name = name;
  //     cmd.ExecuteNonQuery();
  //     conn.Close();
  //   }
  //   //Add book's id and Patron's id to books_Patrons table
  //   public void AddBook(Book newBook)
  //   {
  //     SqlConnection conn = DB.Connection();
  //     conn.Open();
  //
  //     SqlCommand cmd = new SqlCommand("INSERT INTO books_Patrons (book_id, Patron_id) VALUES (@BookId, @PatronId);", conn);
  //
  //     SqlParameter bookIdParameter = new SqlParameter( "@BookId", newBook.GetId());
  //     SqlParameter PatronIdParameter = new SqlParameter("@PatronId", this.GetId());
  //
  //     cmd.Parameters.Add(bookIdParameter);
  //     cmd.Parameters.Add(PatronIdParameter);
  //     cmd.ExecuteNonQuery();
  //     if (conn != null)
  //     {
  //       conn.Close();
  //     }
  //   }
  //
  //   public void Delete()
  //   {
  //     SqlConnection conn = DB.Connection();
  //     conn.Open();
  //
  //     SqlCommand cmd = new SqlCommand("DELETE FROM Patrons WHERE id = @Id; DELETE FROM books_Patrons WHERE Patron_id = @Id;", conn);
  //     SqlParameter IdParameter = new SqlParameter("@Id", this.GetId());
  //
  //     cmd.Parameters.Add(IdParameter);
  //     cmd.ExecuteNonQuery();
  //
  //     if (conn != null)
  //     {
  //       conn.Close();
  //     }
  //   }

    // public static void DeleteAll()
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //   SqlCommand cmd = new SqlCommand("DELETE FROM Patrons;", conn);
    //   cmd.ExecuteNonQuery();
    //   conn.Close();
    // }
  // }
}
