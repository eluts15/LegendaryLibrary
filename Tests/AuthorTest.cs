using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Library
{
  [Collection("Library")]
  public class AuthorTest : IDisposable
  {
    public AuthorTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb; Initial Catalog=library_test; Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_AuthorEmptyAtFirst()
    {
      //Arrange, Act
      int result = Author.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Override_ObjectsAreEqual()
    {
      //Arrange, Act
      Author author1 = new Author("Herman Melville");
      Author author2 = new Author("Herman Melville");
      //Assert
      Assert.Equal(author1, author2);
    }

    [Fact]
    public void Test_Save_SaveAuthorToDatabase()
    {
      //Arrange
      Author testAuthor = new Author("J.R.R. Tolkien");
      testAuthor.Save();

      //Act
      List<Author> result = Author.GetAll();
      List<Author> testList = new List<Author>{testAuthor};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Find_FindsAuthorInDatabase()
    {
      //Arrange
      Author testAuthor = new Author("G.R.R. Martin");
      testAuthor.Save();
      //Act
      Author foundAuthor = Author.Find(testAuthor.GetId());
      //Assert
      Assert.Equal(testAuthor, foundAuthor);
    }

    [Fact]
    public void Test_Update_UpdatesAuthorInDatabase()
    {
      //Arrange
      Author testAuthor = new Author("John Steinbeck");
      testAuthor.Save();
      string newName = "Steven Steinbeck";
      //Act
      testAuthor.Update(newName);
      string result = testAuthor.GetName();

      //Assert
      Assert.Equal(newName, result);
    }

    [Fact]
    public void GetBooks_ReturnsAllAuthorBook_BookList()
    {
     //Arrange
     Author testAuthor = new Author("Ursula K. Le Guin");
     testAuthor.Save();

     Book testBook1 = new Book("A Wrinkle In Time", "Fantasy", new DateTime(2019, 08, 15));
     testBook1.Save();

     Book testBook2 = new Book("A Wizard of EarthSea", "Fantasy", new DateTime(2014, 09, 05));
     testBook2.Save();

     //Act
     testAuthor.AddBook(testBook1);
     List<Book> savedBook = testAuthor.GetBooks();
     List<Book> testList = new List<Book> {testBook1};

     //Assert
     Assert.Equal(testList, savedBook);
    }

    [Fact]
    public void Test_AddBook_AddsBookToAuthor()
    {
      //Arrange
      Author testAuthor = new Author("Kurt Vonnegut, Jr.");
      testAuthor.Save();

      Book testBook = new Book("Slaughterhouse Five", "Fiction", new DateTime(2017, 05, 09));
      testBook.Save();

      Book testBook2 = new Book("Breakfast of Champions", "Fiction", new DateTime(2017, 07, 02));
      testBook2.Save();

      //Act
      testAuthor.AddBook(testBook);
      testAuthor.AddBook(testBook2);

      List<Book> result = testAuthor.GetBooks();
      List<Book> testList = new List<Book>{testBook, testBook2};

      //Assert
      Assert.Equal(testList, result);
    }

    // [Fact]
    // public void Delete_DeletesAuthorAssociationsFromDatabase_AuthorList()
    // {
    //   //Arrange
    //   Book testBook = new Book("Expandrew", new DateTime(2016, 10, 20), "Game Art & Design");
    //   testBook.Save();
    //
    //   Author testAuthor = new Author("Sleepology", "SL101", "No", "F");
    //   testAuthor.Save();
    //
    //   //Act
    //   testAuthor.AddBook(testBook);
    //   testAuthor.Delete();
    //
    //   List<Author> resultBookAuthor = testBook.GetAuthors();
    //   List<Author> testBookAuthor = new List<Author> {};
    //
    //   //Assert
    //   Assert.Equal(testBookAuthor, resultBookAuthor);
    // }


    public void Dispose()
    {
      Book.DeleteAll();
      Author.DeleteAll();
    }

  }
}
