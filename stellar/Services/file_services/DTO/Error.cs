﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElFinder.DTO {
    /// <summary> </summary>
    internal static class Error {
        /// <summary> </summary>
        public static JsonResult CommandNotFound() {
            return Json(new { error = "errUnknownCmd" });
        }
        /// <summary> </summary>
        public static JsonResult MissedParameter(string command) {
            return Json(new { error = new string[] { "errCmdParams", command } });
        }
        /// <summary> </summary>
        private static JsonResult Json(object data) {
            return new JsonDataContractResult(data) { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}