using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTemplateLibrary.Interfaces;

namespace DataTemplateLibrary.Models
{
    /// <summary>
    /// Data model for objects from database.
    /// </summary>
    /// <creator>Anton Kalashniko</creator>
    public class DBService : IBaseClass
    {
        private int id = -1;
        private int userId;
        private string serviceName;
        private int currentPrice;
        private DateOnly created;
        private DateOnly? updated;
        private bool isShown;
        private string shortDescription;
        private string? longDescription;
        private string? linkToImage;
        private bool isDeleted;
        public DBService(int iD, int userId, string serviceName, int currentPrice, DateOnly created, DateOnly? updated, bool isShown, string shortDescription, string? longDescription, string? linkToImage, bool isDeleted)
        {
            ID = iD;
            UserId = userId;
            ServiceName = serviceName;
            CurrentPrice = currentPrice;
            Created = created;
            Updated = updated;
            IsShown = isShown;
            ShortDescription = shortDescription;
            LongDescription = longDescription;
            LinkToImage = linkToImage;
            IsDeleted = isDeleted;
        }
        public DBService(int userId, string serviceName, int currentPrice, DateOnly created, DateOnly? updated, bool isShown, string shortDescription, string? longDescription, string? linkToImage, bool isDeleted)
        {
            UserId = userId;
            ServiceName = serviceName;
            CurrentPrice = currentPrice;
            Created = created;
            Updated = updated;
            IsShown = isShown;
            ShortDescription = shortDescription;
            LongDescription = longDescription;
            LinkToImage = linkToImage;
            IsDeleted = isDeleted;
        }
        public int ID { get => id; set => id = value; }
        public int UserId { get => userId; set => userId = value; }
        public string ServiceName { get => serviceName; set => serviceName = value; }
        public int CurrentPrice { get => currentPrice; set => currentPrice = value; }
        public DateOnly Created { get => created; set => created = value; }
        public DateOnly? Updated { get => updated; set => updated = value; }
        public bool IsShown { get => isShown; set => isShown = value; }
        public string ShortDescription { get => shortDescription; set => shortDescription = value; }
        public string? LongDescription { get => longDescription; set => longDescription = value; }
        public string? LinkToImage { get => linkToImage; set => linkToImage = value; }
        public bool IsDeleted { get => isDeleted; set => isDeleted = value; }
    }
}
