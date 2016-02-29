using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace InventoryList
{
  public class Inventory
  {
    private int Id;
    private string Name;

    public Inventory(string name, int id = 0)
    {
      Id = id;
      Name = name;
    }

    public override bool Equals(System.Object otherInventory)
    {
      if(!(otherInventory is Inventory))
      {
        return false;
      }
      else
      {
        Inventory newInventory = (Inventory) otherInventory;
        bool idEquality = (this.GetId() == newInventory.GetId());
        bool nameEquality = (this.GetName() == newInventory.GetName());
        return (idEquality && nameEquality);
      }
    }

    public void SetId(int id)
    {
      Id = id;
    }
    public void SetName(string name)
    {
      Name = name;
    }
    public int GetId()
    {
      return Id;
    }
    public string GetName()
    {
      return Name;
    }
    public static List<Inventory> GetAll()
    {
      List<Inventory> allInventory = new List<Inventory> {};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM inventories;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int inventoryId = rdr.GetInt32(0);
        string inventoryName = rdr.GetString(1);
        Inventory newInventory = new Inventory(inventoryName, inventoryId);
        allInventory.Add(newInventory);
      }

      if(rdr !=null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }

      return allInventory;
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO inventories (name) OUTPUT INSERTED.id VALUES (@InventoryName);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@InventoryName";
      nameParameter.Value = this.GetName();
      cmd.Parameters.Add(nameParameter);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this.Id = rdr.GetInt32(0);
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
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM inventories;", conn);
      cmd.ExecuteNonQuery();
    }
    public static Inventory Find(string name)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM inventories WHERE name = @InventoryName;", conn);
      SqlParameter inventoryParameter = new SqlParameter();
      inventoryParameter.ParameterName = "@InventoryName";
      inventoryParameter.Value = name;
      cmd.Parameters.Add(inventoryParameter);
      rdr = cmd.ExecuteReader();

      int foundInventoryId = 0;
      string foundInventoryName = null;
      while(rdr.Read())
      {
        foundInventoryId = rdr.GetInt32(0);
        foundInventoryName = rdr.GetString(1);
      }
      Inventory foundInventory = new Inventory(foundInventoryName, foundInventoryId);

      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }

      return foundInventory;
    }
  }
}
