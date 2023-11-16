

namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class DependencyImplementation : IDependency
{
    public int Create(Dependency _dependency)
    {
        int newId = DataSource.Config.NextDependencyId;
        Dependency dependency = _dependency with { Id = newId };
        //throw new NotImplementedException();
        DataSource.Dependencies.Add(dependency);
        return newId;
    }

    public void Delete(int id)
    {
        Dependency? d = DataSource.Dependencies.Find(d => d.Id == id);
        if (d != null)
        {
            DataSource.Dependencies.Remove(d);
        }
        else
        {
            /*throw new NotImplementedException();*/
        }
    }

    public Dependency? Read(int id)
    {
        return DataSource.Dependencies.Find(d => d.Id == id);
    }

    public List<Dependency> ReadAll()
    {
        return new List<Dependency>(DataSource.Dependencies);
    }

    public void Update(Dependency _dependency)
    {
        //throw new NotImplementedException();
        Dependency? d = DataSource.Dependencies.Find(d => d.Id == _dependency.Id);
        if (d != null)
        {
            DataSource.Dependencies.Remove(d);
            DataSource.Dependencies.Add(_dependency);
        }
        else
        {
            //throw new InvalidOperationException();
        }
    }
}
