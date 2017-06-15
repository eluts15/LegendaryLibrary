using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Library
{
  public class Copy
  {
    private int _id;
    private string _name;
    private DateTime _due;

    public Copy(string name, DateTime due, int id = 0)
    {
      _name = name;
      _due = due;
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
    public DateTime GetDue()
    {
      return _due;
    }

    public override bool Equals(System.Object otherCopy)
    {
      if(!(otherCopy is Copy))
      {
        return false;
      }
      else
      {
        Copy newCopy = (Copy) otherCopy;
        bool idEquality = (this.GetId() == newCopy.GetId());
        bool nameEquality = (this.GetName() == newCopy.GetName());
        bool dueEquality = (this.GetDue() == newCopy.GetDue());
        return (idEquality && nameEquality && dueEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.GetName().GetHashCode();
    }

    public static List<Copy> GetAll()
    {
      List<Copy> AllCopy = new List<Copy>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM copies", conn);
      SqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        DateTime due = rdr.GetDateTime(2);
        Copy newCopy = new Copy(name, due, id);
        AllCopy.Add(newCopy);
      }
      if (rdr != null)
      {
       rdr.Close();
      }
      if (conn != null)
      {
       conn.Close();
      }
      return AllCopy;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO copies (name, due) OUTPUT INSERTED.id VALUES (@name, @due);", conn);

      SqlParameter namePara = new SqlParameter("@name", this.GetName());
      SqlParameter duePara = new SqlParameter("@due", this.GetDue());

      cmd.Parameters.Add(namePara);
      cmd.Parameters.Add(duePara);

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

    public static Copy Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM copies WHERE id = @id;", conn);
      SqlParameter idParameter = new SqlParameter("@id", id.ToString());

      cmd.Parameters.Add(idParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundId = 0;
      string name = null;
      DateTime due = new DateTime();

      while(rdr.Read())
      {
        foundId = rdr.GetInt32(0);
        name = rdr.GetString(1);
        due = rdr.GetDateTime(2);
      }
      Copy foundCopy = new Copy(name, due, foundId);
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundCopy;
    }
    //
    public void Update(string name, DateTime due)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE copies SET name = @name WHERE id = @Id; UPDATE copies SET due = @due WHERE id = @Id;", conn);

      SqlParameter namePara = new SqlParameter("@name", name);
      SqlParameter duePara = new SqlParameter("@due", due);
      SqlParameter idPara = new SqlParameter("@Id", this.GetId());

      cmd.Parameters.Add(namePara);
      cmd.Parameters.Add(idPara);
      cmd.Parameters.Add(duePara);

      this._name = name;
      this._due = due;
      cmd.ExecuteNonQuery();
      conn.Close();
    }


    public List<Patron> GetPatrons()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT patrons.* FROM copies JOIN patrons_copies ON (copies.id = patrons_copies.copy_id) JOIN patrons ON (patrons_copies.patron_id = patrons.id) WHERE copies.id = @copiesId;", conn);
      SqlParameter copiesIdParam = new SqlParameter("@copiesId", this.GetId().ToString());

      cmd.Parameters.Add(copiesIdParam);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Patron> patrons = new List<Patron>{};

      while(rdr.Read())
      {
        int patronId = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        Patron newPatron = new Patron(name, patronId);
        patrons.Add(newPatron);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return patrons;
    }
    //Add copy's id and patron's id to patrons_copies table
    public void AddPatron(Patron newPatron)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO patrons_copies (patron_id, copy_id) VALUES (@PatronId, @CopyId);", conn);

      SqlParameter copyIdParameter = new SqlParameter("@CopyId", this.GetId());
      SqlParameter patronIdParameter = new SqlParameter( "@PatronId", newPatron.GetId());

      cmd.Parameters.Add(copyIdParameter);
      cmd.Parameters.Add(patronIdParameter);
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

      SqlCommand cmd = new SqlCommand("DELETE FROM copies WHERE id = @copyId; DELETE FROM patrons_copies WHERE copy_id = @copyId;", conn);
      SqlParameter copyIdParameter = new SqlParameter("@copyId", this.GetId());

      cmd.Parameters.Add(copyIdParameter);
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
     SqlCommand cmd = new SqlCommand("DELETE FROM copies;", conn);
     cmd.ExecuteNonQuery();
     conn.Close();
    }
  }
}
