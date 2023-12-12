﻿namespace DalApi;

public interface ICrud<T> where T : class
{
    int Create(T item);
    T? Read(int id);
    List<T> ReadAll();
    void Update(T item);
    void Delete(int id);
}

