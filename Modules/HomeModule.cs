using Nancy;
using System.Collections.Generic;
using System;

namespace InventoryList
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        List<Inventory> allInventories = Inventory.GetAll();
        return View["index.cshtml", allInventories];
      };
      Get["/item/new"] = _ => {
        return View["item_form.cshtml"];
      };
      Post["/item/new"] = _ => {
        Inventory newInventory = new Inventory(Request.Form["new-item"]);
        newInventory.Save();
        return View["item.cshtml", newInventory];
      };
      Get["/item/delete"] = _ => {
        Inventory.DeleteAll();
        return View["item-deleted.cshtml"];
      };
      Get["/item/search"] = _ => {
        return View["item-search.cshtml"];
      };
      Post["/item/search/results"] = _ => {
        string inventoryName = Request.Form["search-item"];
        var found = Inventory.Find(inventoryName);
        return View["search-results.cshtml", found];
      };
    }
  }
}
