using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NamespaceGPT.Data.Models;

namespace NamespaceGPT.Business.Services.Interfaces
{
    public interface IMarketplaceService
    {
        int AddMarketplace(Marketplace marketplace);
        bool DeleteMarketplace(int id);
        bool UpdateMarketplace(int id, Marketplace marketplace);
        IEnumerable<Marketplace> GetAllMarketplaces();
        Marketplace? GetMarketplace(int id);
    }
}
