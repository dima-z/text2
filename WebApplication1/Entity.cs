using System;

namespace WebApplication
{
  public class Entity
  {
    public Guid Id
    {
      get; set;
    }

    public DateTime OperationDate
    {
      get; set;
    }

    public decimal Amount
    {
      get; set;
    }

    public Entity()
    {
      Id = Guid.NewGuid();
      OperationDate = DateTime.Now;
    }
  }

}
