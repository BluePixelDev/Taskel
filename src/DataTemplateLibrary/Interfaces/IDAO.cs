using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTemplateLibrary.Interfaces
{
    /// <summary>
    /// Interface for objects that work with database to all have basic methods for data manipulaiton with database.
    /// </summary>
    /// <typeparam name="T">DAO object</typeparam>
    /// <creator>Anton Kalashnikov</creator>
    public interface IDAO<T> where T : IBaseClass
    {
        T? GetByID(int id);
        List<T> GetAll();
        int Create(T element);
        void Save(T element);
        void Delete(int id);
    }
}
