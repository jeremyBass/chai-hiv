﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using stellar.Models;

namespace stellar {
    /// <summary>
    /// This class is the interface for the exposed OAuth API.
    /// </summary>
    [ServiceContract]
    public interface IService1 {

        [OperationContract]
        [WebGet]
        string GetData();


        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        appuser[] GetAccounts();

    }
}
