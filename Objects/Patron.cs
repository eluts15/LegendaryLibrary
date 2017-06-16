using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Library
{
  public class Patron
  {
    private int _id;
    private string _name;

    public Patron (string name, int id = 0)
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

    public override bool Equals(System.Object otherPatron)
    {
     if(!(otherPatron is Patron))
     {
       return false;
     }
     else
      {
       Patron newPatron = (Patron) otherPatron;
       bool idEquality = (this.GetId() == newPatron.GetId());
       bool nameEquality = (this.GetName() == newPatron.GetName());
       return (idEquality && nameEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.GetName().GetHashCode();
    }

    public static List<Patron> GetAll()
    {
      List<Patron> AllPatron = new List<Patron>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM patrons;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        Patron newPatron = new Patron(name, id);
        AllPatron.Add(newPatron);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return AllPatron;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO patrons (name) OUTPUT INSERTED.id VALUES (@name);", conn);

      SqlParameter nameParam = new SqlParameter("@name", this.GetName());

      cmd.Parameters.Add(nameParam);
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

    public static Patron Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM patrons WHERE id = @id;", conn);
      SqlParameter IdParam = new SqlParameter("@id", id.ToString());

      cmd.Parameters.Add(IdParam);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundId = 0;
      string name = null;

      while(rdr.Read())
      {
        foundId = rdr.GetInt32(0);
        name = rdr.GetString(1);
      }
      Patron foundPatron = new Patron(name, foundId);
      if (rdr != null)
      {
       rdr.Close();
      }
      if (conn != null)
      {
       conn.Close();
      }
     return foundPatron;
    }

    public void Update(string name)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE patrons SET name = @name WHERE id = @Id;", conn);

      SqlParameter nameParam = new SqlParameter("@name", name);
      SqlParameter idParam = new SqlParameter("@Id", this.GetId());

      cmd.Parameters.Add(nameParam);
      cmd.Parameters.Add(idParam);

      this._name = name;
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public List<Copy> GetCopies()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT copies.* FROM patrons JOIN patrons_copies ON (patrons.id = patrons_copies.patron_id) JOIN copies ON (patrons_copies.copy_id = copies.id) WHERE patrons.id = @PatronId;", conn);
      SqlParameter patronIdParam = new SqlParameter("@PatronId", this.GetId().ToString());

      cmd.Parameters.Add(patronIdParam);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Copy> Copies = new List<Copy>{};

      while(rdr.Read())
      {
        int copyId = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        DateTime due = rdr.GetDateTime(2);
        Copy newCopy = new Copy(name, due, copyId);
        Copies.Add(newCopy);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return Copies;
    }

    //Add Copy's id and Patron's id to patrons_copies table
    public void AddCopy(Copy newCopy)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO patrons_copies (patron_id, copy_id) VALUES (@PatronId, @CopyId);", conn);

      SqlParameter copyIdParam = new SqlParameter( "@CopyId", newCopy.GetId());
      SqlParameter patronIdParam = new SqlParameter("@PatronId", this.GetId());

      cmd.Parameters.Add(copyIdParam);
      cmd.Parameters.Add(patronIdParam);
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

      SqlCommand cmd = new SqlCommand("DELETE FROM Patrons WHERE id = @Id; DELETE FROM patrons_copies WHERE Patron_id = @Id;", conn);
      SqlParameter IdParam = new SqlParameter("@Id", this.GetId());

      cmd.Parameters.Add(IdParam);
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
      SqlCommand cmd = new SqlCommand("DELETE FROM Patrons;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
