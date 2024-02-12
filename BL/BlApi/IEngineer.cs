using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface IEngineer
{
    public IEnumerable<BO.Engineer> ReadAllEngineers(Func<BO.Engineer, bool>? filter);
    public BO.Engineer GetEngineer(int id);
    public void CreateEngineer(BO.Engineer engineer);
    public void DeleteEngineer(int id);
    public void UpdateEngineer(BO.Engineer engineer);
}
