using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace InventoryList
{
  public class InventoryTest : IDisposable
  {
    public InventoryTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=inventoryList_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Inventory.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfDescriptionsAreTheSame()
    {
      //Arrange, Act
      Inventory firstInventory = new Inventory("Nexus 5");
      Inventory secondInventory = new Inventory("Nexus 5");

      //Assert
      Assert.Equal(firstInventory, secondInventory);
    }

    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      //Ararnge
      Inventory testInventory = new Inventory("Nexus 5");

      //Act
      testInventory.Save();
      List<Inventory> result = Inventory.GetAll();
      List<Inventory> testList = new List<Inventory>{testInventory};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_SAVE_AssignsIdToObject()
    {
      //Arrange
      Inventory testInventory = new Inventory("Nexeus 5");

      //Act
      testInventory.Save();
      Inventory savedInventory = Inventory.GetAll()[0];

      int result = savedInventory.GetId();
      int testId = testInventory.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsInventoryInDatabase()
    {
      //Ararnge
      Inventory testInventory = new Inventory("Nexus 5");
      testInventory.Save();

      //Act
      Inventory foundInventory = Inventory.Find(testInventory.GetName());

      //Assert
      Assert.Equal(testInventory, foundInventory);
    }

    public void Dispose()
    {
      Inventory.DeleteAll();
    }
  }
}
