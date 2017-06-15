using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Library
{
  [Collection("Library")]
  public class PatronTest : IDisposable
  {
    public PatronTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb; Initial Catalog=library_test; Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_PatronEmptyAtFirst()
    {
      //Arrange, Act
      int result = Patron.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }
    
    // [Fact]
    // public void Test_Override_ObjectsAreEqual()
    // {
    //   //Arrange, Act
    //   Patron Patron1 = new Patron("Herman Melville");
    //   Patron Patron2 = new Patron("Herman Melville");
    //   //Assert
    //   Assert.Equal(Patron1, Patron2);
    // }
    //
    // [Fact]
    // public void Test_Save_SavePatronToDatabase()
    // {
    //   //Arrange
    //   Patron testPatron = new Patron("J.R.R. Tolkien");
    //   testPatron.Save();
    //
    //   //Act
    //   List<Patron> result = Patron.GetAll();
    //   List<Patron> testList = new List<Patron>{testPatron};
    //
    //   //Assert
    //   Assert.Equal(testList, result);
    // }
    //
    // [Fact]
    // public void Test_Find_FindsPatronInDatabase()
    // {
    //   //Arrange
    //   Patron testPatron = new Patron("G.R.R. Martin");
    //   testPatron.Save();
    //   //Act
    //   Patron foundPatron = Patron.Find(testPatron.GetId());
    //   //Assert
    //   Assert.Equal(testPatron, foundPatron);
    // }
    //
    // [Fact]
    // public void Test_Update_UpdatesPatronInDatabase()
    // {
    //   //Arrange
    //   Patron testPatron = new Patron("John Steinbeck");
    //   testPatron.Save();
    //   string newName = "Steven Steinbeck";
    //   //Act
    //   testPatron.Update(newName);
    //   string result = testPatron.GetName();
    //
    //   //Assert
    //   Assert.Equal(newName, result);
    // }
    //
    // [Fact]
    // public void GetBooks_ReturnsAllPatronBook_BookList()
    // {
    //  //Arrange
    //  Patron testPatron = new Patron("Ursula K. Le Guin");
    //  testPatron.Save();
    //
    //  Book testBook1 = new Book("A Wrinkle In Time", "Fantasy", new DateTime(2019, 08, 15));
    //  testBook1.Save();
    //
    //  Book testBook2 = new Book("A Wizard of EarthSea", "Fantasy", new DateTime(2014, 09, 05));
    //  testBook2.Save();
    //
    //  //Act
    //  testPatron.AddBook(testBook1);
    //  List<Book> savedBook = testPatron.GetBooks();
    //  List<Book> testList = new List<Book> {testBook1};
    //
    //  //Assert
    //  Assert.Equal(testList, savedBook);
    // }
    //
    // [Fact]
    // public void Test_AddBook_AddsBookToPatron()
    // {
    //   //Arrange
    //   Patron testPatron = new Patron("Kurt Vonnegut, Jr.");
    //   testPatron.Save();
    //
    //   Book testBook = new Book("Slaughterhouse Five", "Fiction", new DateTime(2017, 05, 09));
    //   testBook.Save();
    //
    //   Book testBook2 = new Book("Breakfast of Champions", "Fiction", new DateTime(2017, 07, 02));
    //   testBook2.Save();
    //
    //   //Act
    //   testPatron.AddBook(testBook);
    //   testPatron.AddBook(testBook2);
    //
    //   List<Book> result = testPatron.GetBooks();
    //   List<Book> testList = new List<Book>{testBook, testBook2};
    //
    //   //Assert
    //   Assert.Equal(testList, result);
    // }
    //
    // [Fact]
    // public void Delete_DeletesPatronAssociationsFromDatabase_PatronList()
    // {
    //   //Arrange
    //   Book testBook = new Book("The Unbearable Lightness of Being", "Realistic Fiction", new DateTime(2014, 04, 02));
    //   testBook.Save();
    //
    //   Patron testPatron = new Patron("Milan Kundera");
    //   testPatron.Save();
    //
    //   //Act
    //   testPatron.AddBook(testBook);
    //   testPatron.Delete();
    //
    //   List<Patron> resultBookPatron = testBook.GetPatrons();
    //   List<Patron> testBookPatron = new List<Patron> {};
    //
    //   //Assert
    //   Assert.Equal(testBookPatron, resultBookPatron);
    // }


    public void Dispose()
    {
      Book.DeleteAll();
      Author.DeleteAll();
      Copy.DeleteAll();
      // Patron.DeleteAll();
    }
  }
}
