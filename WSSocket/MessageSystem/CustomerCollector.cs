/****************************************
***创建人：bhlfy
***创建时间：2014-11-20 15:00:29
***公司：龙浩通信公司
***文件描述：在线用户实体集合。
*****************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace WSSocket
{
    public class CustomerCollector
    {
        public object _CustomLocker = true;
        private List<Customer> _customers;

        public List<Customer> _Customers
        {
            get { return _customers; }
            set { _customers = value; }
        }

        public CustomerCollector()
        {
            if (_Customers == null)
                _Customers = new List<Customer>();
        }

        private string _findUid = "";
        private Customer newCustomer = null;

        private bool existContent(Customer oldCustomer)
        {
            return ((newCustomer._UId == oldCustomer._UId) &&
                (newCustomer.IPAddress == oldCustomer.IPAddress) &&
                (newCustomer.Port == oldCustomer.Port));
                
        }

        public Customer IsExist(Customer customer)
        {
            #region
            lock(customer)
            { 
                newCustomer = customer;
                if (_Customers.Count > 0)
                    return _Customers.Find(existContent);
                else
                    return null;
            }
            #endregion
        }

        private bool matchUID(Customer oldCustomer)
        {
            #region

            return (_findUid == oldCustomer._UId.ToString());
            #endregion
        }

        public List<Customer> FindCustomers(string UId)
        {
            #region
            _findUid = UId;
            return _Customers.FindAll(matchUID);
            #endregion
        }

        public void Add(Customer customer)
        {
            #region
            _Customers.Add(customer);
            #endregion
        }
        public void Remove(Customer customer)
        {
            #region
            foreach (Customer temp in _customers)
            {
                if (temp._UId == customer._UId &&
                    temp.IPAddress == customer.IPAddress &&
                    temp.Port == customer.Port)
                {
                    _customers.Remove(temp);
                    break;
                }
            }
            #endregion
        }
        public void Remove(Socket customerSocket)
        {
            #region
            System.Net.IPEndPoint endremotepoint = (System.Net.IPEndPoint)customerSocket.RemoteEndPoint;
            string ipaddress = "";
            int port = -1;
            ipaddress = endremotepoint.Address.ToString();
            port = endremotepoint.Port;
            foreach (Customer temp in _customers)
            {
                if (temp.IPAddress == ipaddress &&
                    temp.Port == port)
                {
                    _customers.Remove(temp);
                    break;
                }
            }
            #endregion
        }
        public void ViewToConsole()
        {
            Console.Write(ToString());
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            object[] param = new object[5];
            foreach (Customer temp in _customers)
            {
                param[0] = temp._UId;
                param[1] = temp.IPAddress;
                param[2] = temp.Port;
                param[3] = temp._LogonTime;
                param[4] = temp._UpdateTime;

                str.AppendLine(string.Format("UID:{0},IPAddress:{1},ClientPort:{2},LoginTime:{3},UpdateTime:{4}",
                    param));
            }
            return str.ToString();
        }

        public void UpdateUserTime(Customer customer)
        {
            lock (typeof(CustomerCollector))
            { 
                Customer findcustomer = this.IsExist(customer);
                if (findcustomer != null)
                    findcustomer._UpdateTime = DateTime.Now;
            }
        }
        
    }
}
