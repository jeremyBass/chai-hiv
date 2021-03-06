﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth.Messages;

namespace stellar.Tools {
    /// <summary> </summary>
    public class RequestScopedTokenMessage : UnauthorizedTokenRequest {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestScopedTokenMessage"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint that will receive the message.</param>
        /// <param name="version">The OAuth version.</param>
        public RequestScopedTokenMessage(MessageReceivingEndpoint endpoint, Version version)
            : base(endpoint, version) {
        }

        /// <summary>
        /// Gets or sets the scope of the access being requested.
        /// </summary>
        [MessagePart("scope", IsRequired = true)]
        public string Scope { get; set; }
    }
}