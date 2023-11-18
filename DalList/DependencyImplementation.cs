

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
        DataSource.Dependencies.Add(dependency);
        return newId;
    }

    public void Delete(int id)
    {
        Dependency? d = DataSource.Dependencies.Find(d => d?.Id == id);
        if (d != null)
        {
            DataSource.Dependencies.Remove(d);
        }
        else
        {
            throw new Exception($"Can't delete, Dependency with ID: {id} does not exist!!");
        }
    }

    public Dependency? Read(int id)
    {
        return DataSource.Dependencies.Find(d => d?.Id == id);
    }

    public List<Dependency> ReadAll()
    {
        return new List<Dependency>(DataSource.Dependencies!/*?*/);
    }

    public void Update(Dependency? _dependency)
    {
        Dependency? d = DataSource.Dependencies.Find(d => d?.Id == _dependency?.Id);
        if (d != null)
        {
            DataSource.Dependencies.Remove(d);
            DataSource.Dependencies.Add(_dependency);
        }
        else
        {
            throw new Exception($"Can't update, Dependency with ID: {_dependency?.Id} does not exist!!");
        }
    }
}
