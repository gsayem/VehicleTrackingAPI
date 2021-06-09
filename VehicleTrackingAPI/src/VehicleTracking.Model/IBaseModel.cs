using System;

namespace VehicleTracking.Model
{
    public interface IBaseModel
    {
        Guid Id { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime UpdatedDate { get; set; }        
        /// <summary>
        /// Trigger before create a data
        /// </summary>
        void OnCreate();
        /// <summary>
        /// Trigger before update a data
        /// </summary>
        void OnUpdate();
        /// <summary>
        /// Trigger before delete a data
        /// </summary>
        void OnDelete();
    }
}
