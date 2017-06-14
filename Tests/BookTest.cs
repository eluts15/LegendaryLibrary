using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Library
{
  [Collection("Library")]
  public class BookTest : IDisposable
  {
    public BookTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb; Initial Catalog=library_test; Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
     //Arrange, Act
     int result = Book.GetAll().Count;

     //Assert
     Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Override_ObjectsAreEqual()
    {
      //Arrange, Act
      Book book1 = new Book("Moby Dick'd", "required reading", new DateTime(1851, 11, 14));
      Book book2 = new Book("Moby Dick'd", "required reading", new DateTime(1851, 11, 14));
      //Assert
      Assert.Equal(book1, book2);
    }

    [Fact]
    public void Test_Save_SavesToDatabase()
    {
     //Arrange
    Book testBook = new Book("The Old Man and the Sea", "Literature", new DateTime(2017, 02, 28));

     //Act
     testBook.Save();
     List<Book> result =Book.GetAll();
     List<Book> testList = new List<Book>{testBook};

     //Assert
     Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Find_FindBookInDatabase()
    {
      //Arrange
      Book testBook = new Book("The Hobbit", "Fantasy", new DateTime(2017, 07, 21));
      testBook.Save();

      //Act
      Book foundBook = Book.Find(testBook.GetId());

      //Assert
      Assert.Equal(testBook, foundBook);
    }

    [Fact]
    public void Test_Update_UpdatesBookInDatabase()
    {
      //Arrange
      Book testBook = new Book("The Art of Deal", "Actual Garbage", new DateTime(2017, 09, 27));
      testBook.Save();
      DateTime newDueDate = new DateTime(2017, 12, 03);
      //Act
      testBook.Update("The Art of Deal", "Actual Garbage", new DateTime(2017, 12, 03));
      DateTime result = testBook.GetDueDate();

      //Assert
      Assert.Equal(newDueDate, result);
    }


    [Fact]
    public void GetAuthors_ReturnsAllBookAuthors_AuthorList()
    {
      //Arrange
      Book testBook = new Book("The C Programming Language", "Reference Book", new DateTime(2018, 03, 16));
      testBook.Save();

      Author testAuthors1 = new Author("Biran W. Kernighan");
      testAuthors1.Save();

      Author testAuthors2 = new Author("Dennis M. Ritchie");
      testAuthors2.Save();

      //Act
      testBook.AddAuthor(testAuthors1);
      List<Author> result = testBook.GetAuthors();
      List<Author> testList = new List<Author> {testAuthors1};

      //Assert
      Assert.Equal(testList, result);
    }
    // [Fact]
    // public void AddAuthors_AddsAuthorsToBook_AuthorsList()
    // {
    //   //Arrange
    //   Book testBook = new Book("Steven", new DateTime(1984, 12, 25), "Gun Economics");
    //   testBook.Save();
    //
    //   Author testAuthors = new Author("Sleepology", "SL101", "No", "F");
    //   testAuthors.Save();
    //
    //   //Act
    //   testBook.AddAuthor(testAuthors);
    //
    //   List<Author> result = testBook.GetAuthors();
    //   List<Author> testList = new List<Author>{testAuthors};
    //
    //   //Assert
    //
    //   Assert.Equal(testList, result);
    // }
    //
    // [Fact]
    // public void Delete_DeletesBooksAssociationsFromDatabase_BooksList()
    // {
    //   //Arrange
    //   Author testAuthor = new Author("Sleepology", "SL101", "No", "F");
    //   testAuthor.Save();
    //
    //   Book testBooks = new Book("Steven", new DateTime(1984, 12, 25), "Gun Economics");
    //   testBooks.Save();
    //
    //   //Act
    //   testBooks.AddAuthor(testAuthor);
    //   testBooks.Delete();
    //
    //   List<Book> resultAuthorBooks = testAuthor.GetBook();
    //   List<Book> testAuthorBooks = new List<Book> {};
    //
    //   //Assert
    //   Assert.Equal(testAuthorBooks, resultAuthorBooks);
    // }

    public void Dispose()
    {
      Book.DeleteAll();
      Author.DeleteAll();
    }


  }
}
