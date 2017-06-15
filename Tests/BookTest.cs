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
      Book book1 = new Book("Moby Dick'd", "required reading");
      Book book2 = new Book("Moby Dick'd", "required reading");
      //Assert
      Assert.Equal(book1, book2);
    }

    [Fact]
    public void Test_Save_SavesToDatabase()
    {
     //Arrange
    Book testBook = new Book("The Old Man and the Sea", "Literature");

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
      Book testBook = new Book("The Hobbit", "Fantasy");
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
      Book testBook = new Book("The Art of Deal", "Actual Garbage");
      testBook.Save();
      string newGenre = "Burn this";
      //Act
      testBook.Update("The Art of Deal", "Burn this");
      string result = testBook.GetGenre();

      //Assert
      Assert.Equal(newGenre, result);
    }


    [Fact]
    public void GetAuthors_ReturnsAllBookAuthors_AuthorList()
    {
      //Arrange
      Book testBook = new Book("The C Programming Language", "Reference Book");
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
    [Fact]
    public void AddAuthor_AddsAuthorsToBook_AuthorsList()
    {
      //Arrange
      Book testBook = new Book("The Go Programming Language", "Reference");
      testBook.Save();

      Author testAuthors = new Author("Alan A. A. Donvoan");
      testAuthors.Save();

      //Act
      testBook.AddAuthor(testAuthors);

      List<Author> result = testBook.GetAuthors();
      List<Author> testList = new List<Author>{testAuthors};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Delete_DeletesBooksAssociationsFromDatabase_BooksList()
    {
      //Arrange
      Author testAuthor = new Author("Stephen King");
      testAuthor.Save();

      Book testBooks = new Book("It", "Horror");
      testBooks.Save();

      //Act
      testBooks.AddAuthor(testAuthor);
      testBooks.Delete();

      List<Book> resultAuthorBooks = testAuthor.GetBooks();
      List<Book> testAuthorBooks = new List<Book> {};

      //Assert
      Assert.Equal(testAuthorBooks, resultAuthorBooks);
    }

    public void Dispose()
    {
      Book.DeleteAll();
      Author.DeleteAll();
      Copy.DeleteAll();
      Patron.DeleteAll();
    }
  }
}
