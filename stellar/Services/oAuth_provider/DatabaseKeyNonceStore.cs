﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using DotNetOpenAuth.Messaging.Bindings;
using stellar;


namespace stellar.oauth.Code {


	/// <summary>
	/// A database-persisted nonce store.
	/// </summary>
	public class DatabaseKeyNonceStore : INonceStore, ICryptoKeyStore {
		/// <summary>
		/// Initializes a new instance of the <see cref="DatabaseKeyNonceStore"/> class.
		/// </summary>
		public DatabaseKeyNonceStore() {
		}

		#region INonceStore Members

		/// <summary>
		/// Stores a given nonce and timestamp.
		/// </summary>
		/// <param name="context">The context, or namespace, within which the
		/// <paramref name="nonce"/> must be unique.
		/// The context SHOULD be treated as case-sensitive.
		/// The value will never be <c>null</c> but may be the empty string.</param>
		/// <param name="nonce">A series of random characters.</param>
		/// <param name="timestampUtc">The UTC timestamp that together with the nonce string make it unique
		/// within the given <paramref name="context"/>.
		/// The timestamp may also be used by the data store to clear out old nonces.</param>
		/// <returns>
		/// True if the context+nonce+timestamp (combination) was not previously in the database.
		/// False if the nonce was stored previously with the same timestamp and context.
		/// </returns>
		/// <remarks>
		/// The nonce must be stored for no less than the maximum time window a message may
		/// be processed within before being discarded as an expired message.
		/// This maximum message age can be looked up via the
		/// property, accessible via the 
		/// property.
		/// </remarks>
		public bool StoreNonce(string context, string nonce, DateTime timestampUtc) {
            GlobalApplication.DataContext.Nonces.InsertOnSubmit(new Nonce { Context = context, Code = nonce, Timestamp = timestampUtc });
			try {
                GlobalApplication.DataContext.SubmitChanges();
				return true;
			} catch (System.Data.Linq.DuplicateKeyException) {
				return false;
			} catch (SqlException) {
				return false;
			}
		}

		#endregion

		#region ICryptoKeyStore Members

        /// <summary> </summary>
		public CryptoKey GetKey(string bucket, string handle) {
			// It is critical that this lookup be case-sensitive, which can only be configured at the database.
            var matches = from key in GlobalApplication.DataContext.SymmetricCryptoKeys
						  where key.Bucket == bucket && key.Handle == handle
						  select new CryptoKey(key.Secret, key.ExpiresUtc.AsUtc());

			return matches.FirstOrDefault();
		}

        /// <summary> </summary>
		public IEnumerable<KeyValuePair<string, CryptoKey>> GetKeys(string bucket) {
            return from key in GlobalApplication.DataContext.SymmetricCryptoKeys
				   where key.Bucket == bucket
				   orderby key.ExpiresUtc descending
				   select new KeyValuePair<string, CryptoKey>(key.Handle, new CryptoKey(key.Secret, key.ExpiresUtc.AsUtc()));
		}

        /// <summary> </summary>
		public void StoreKey(string bucket, string handle, CryptoKey key) {
			var keyRow = new SymmetricCryptoKey() {
				Bucket = bucket,
				Handle = handle,
				Secret = key.Key,
				ExpiresUtc = key.ExpiresUtc,
			};

            GlobalApplication.DataContext.SymmetricCryptoKeys.InsertOnSubmit(keyRow);
            GlobalApplication.DataContext.SubmitChanges();
		}

        /// <summary> </summary>
		public void RemoveKey(string bucket, string handle) {
            var match = GlobalApplication.DataContext.SymmetricCryptoKeys.FirstOrDefault(k => k.Bucket == bucket && k.Handle == handle);
			if (match != null) {
                GlobalApplication.DataContext.SymmetricCryptoKeys.DeleteOnSubmit(match);
			}
		}

		#endregion
	}
}