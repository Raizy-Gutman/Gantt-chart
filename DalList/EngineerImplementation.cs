

namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class EngineerImplementation : IEngineer
{
    public int Create(Engineer _engineer)
    {
        if (DataSource.Engineers.Find(e => e.Id == _engineer.Id) != null)
        {
            throw new NotImplementedException();
        }
        else
        {
            DataSource.Engineers.Add(_engineer);
            return _engineer.Id;
        }
    }

    public void Delete(int id)
    {
        //throw new NotImplementedException();
        Engineer? e = DataSource.Engineers.Find(e => e.Id == id);
        if (e != null)
        {
            DataSource.Engineers.Remove(e);
            //אם צריך לעדכן לא פעיל- נוסיף שדה מתאים...
        }
        else
        {
            /*throw new NotImplementedException();*/
        }
    }

    public Engineer? Read(int id)
    {
        return DataSource.Engineers.Find(e => e.Id == id);
    }

    public List<Engineer> ReadAll()
    {
        return new List<Engineer>(DataSource.Engineers);
    }

    public void Update(Engineer _engineer)
    {
        //throw new NotImplementedException();
        Engineer? e = DataSource.Engineers.Find(e => e.Id == _engineer.Id);
        if (e != null)
        {
            DataSource.Engineers.Remove(e);
            DataSource.Engineers.Add(_engineer);
        }
        else
        {
            //throw new InvalidOperationException();
        }
    }
}
