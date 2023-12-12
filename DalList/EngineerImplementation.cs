

namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class EngineerImplementation : IEngineer
{
    public int Create(Engineer? _engineer)
    {
        if (DataSource.Engineers.Find(e => e?.Id == _engineer?.Id) != null)
        {
            throw new Exception($"The new engineer cannot be created, an engineer with ID: {_engineer?.Id} already exists in the system.");
        }
        else
        {
            DataSource.Engineers.Add(_engineer);
            return _engineer!.Id;
        }
    }

    public void Delete(int id)
    {
        Engineer? e = DataSource.Engineers.Find(e => e?.Id == id);
        if (e != null)
        {
            DataSource.Engineers.Remove(e);
            //אם צריך לעדכן לא פעיל- נוסיף שדה מתאים...
        }
        else
        {
            throw new Exception($"Can't delete, engineer with ID: {id} does not exist!!");
        }
    }

    public Engineer? Read(int id)
    {
        return DataSource.Engineers.Find(e => e?.Id == id);
    }

    public List<Engineer> ReadAll()
    {
        return new List<Engineer>(DataSource.Engineers!/*?*/);
    }

    public void Update(Engineer? _engineer)
    {
        Engineer? e = DataSource.Engineers.Find(e => e?.Id == _engineer?.Id);
        if (e != null)
        {
            DataSource.Engineers.Remove(e);
            DataSource.Engineers.Add(_engineer);
        }
        else
        {
            throw new Exception($"Can't update, engineer with ID: {_engineer?.Id} does not exist!!");
        }
    }
}
