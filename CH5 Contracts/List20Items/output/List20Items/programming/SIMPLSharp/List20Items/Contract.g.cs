using System;
using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro;

namespace List20Items
{
    /// <summary>
    /// Common Interface for Root Contracts.
    /// </summary>
    public interface IContract
    {
        object UserObject { get; set; }
        void AddDevice(BasicTriListWithSmartObject device);
        void RemoveDevice(BasicTriListWithSmartObject device);
    }

    /// <summary>
    /// List with 20 items
    /// </summary>
    public class Contract : IContract, IDisposable
    {
        #region Components

        private ComponentMediator ComponentMediator { get; set; }

        public List20Items.ListItem.IItem[] Contact { get { return InternalContact.Cast<List20Items.ListItem.IItem>().ToArray(); } }
        private List20Items.ListItem.Item[] InternalContact { get; set; }

        #endregion

        #region Construction and Initialization

        private static readonly IDictionary<int, uint> ContactSmartObjectIdMappings = new Dictionary<int, uint>{
            { 0, 1 }, { 1, 2 }, { 2, 3 }, { 3, 4 }, { 4, 5 }, { 5, 6 }, { 6, 7 }, { 7, 8 }, { 8, 9 }, { 9, 10 }, { 10, 11 }, { 11, 12 }, { 12, 13 }, 
            { 13, 14 }, { 14, 15 }, { 15, 16 }, { 16, 17 }, { 17, 18 }, { 18, 19 }, { 19, 20 }};

        public Contract()
            : this(new List<BasicTriListWithSmartObject>().ToArray())
        {
        }

        public Contract(BasicTriListWithSmartObject device)
            : this(new [] { device })
        {
        }

        public Contract(BasicTriListWithSmartObject[] devices)
        {
            if (devices == null)
                throw new ArgumentNullException("Devices is null");

            ComponentMediator = new ComponentMediator();

            InternalContact = new List20Items.ListItem.Item[ContactSmartObjectIdMappings.Count];
            for (int index = 0; index < ContactSmartObjectIdMappings.Count; index++)
            {
                InternalContact[index] = new List20Items.ListItem.Item(ComponentMediator, ContactSmartObjectIdMappings[index]);
            }

            for (int index = 0; index < devices.Length; index++)
            {
                AddDevice(devices[index]);
            }
        }

        public static void ClearDictionaries()
        {
            ContactSmartObjectIdMappings.Clear();

        }

        #endregion

        #region Standard Contract Members

        public object UserObject { get; set; }

        public void AddDevice(BasicTriListWithSmartObject device)
        {
            for (int index = 0; index < 20; index++)
            {
                InternalContact[index].AddDevice(device);
            }
        }

        public void RemoveDevice(BasicTriListWithSmartObject device)
        {
            for (int index = 0; index < 20; index++)
            {
                InternalContact[index].RemoveDevice(device);
            }
        }

        #endregion

        #region IDisposable

        public bool IsDisposed { get; set; }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;

            for (int index = 0; index < 20; index++)
            {
                InternalContact[index].Dispose();
            }
            ComponentMediator.Dispose(); 
        }

        #endregion

    }
}
