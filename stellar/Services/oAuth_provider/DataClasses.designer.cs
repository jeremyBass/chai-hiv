﻿using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;
using System;
using stellar;
namespace stellar.oauth.Code {



    /// <summary> </summary>
    [global::System.Data.Linq.Mapping.DatabaseAttribute(Name = "Database")]
    public partial class DataClassesDataContext : System.Data.Linq.DataContext {

        private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();

        #region Extensibility Method Definitions
        partial void OnCreated();
        partial void InsertUser(User instance);
        partial void UpdateUser(User instance);
        partial void DeleteUser(User instance);
        partial void InsertClient(Client instance);
        partial void UpdateClient(Client instance);
        partial void DeleteClient(Client instance);
        partial void InsertClientAuthorization(ClientAuthorization instance);
        partial void UpdateClientAuthorization(ClientAuthorization instance);
        partial void DeleteClientAuthorization(ClientAuthorization instance);
        partial void InsertNonce(Nonce instance);
        partial void UpdateNonce(Nonce instance);
        partial void DeleteNonce(Nonce instance);
        partial void InsertSymmetricCryptoKey(SymmetricCryptoKey instance);
        partial void UpdateSymmetricCryptoKey(SymmetricCryptoKey instance);
        partial void DeleteSymmetricCryptoKey(SymmetricCryptoKey instance);
        #endregion

        /// <summary> </summary>
        public DataClassesDataContext() :
            base(global::System.Configuration.ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString, mappingSource) {
            OnCreated();
        }

        /// <summary> </summary>
        public DataClassesDataContext(string connection) :
            base(connection, mappingSource) {
            OnCreated();
        }

        /// <summary> </summary>
        public DataClassesDataContext(System.Data.IDbConnection connection) :
            base(connection, mappingSource) {
            OnCreated();
        }

        /// <summary> </summary>
        public DataClassesDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) :
            base(connection, mappingSource) {
            OnCreated();
        }

        /// <summary> </summary>
        public DataClassesDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) :
            base(connection, mappingSource) {
            OnCreated();
        }

        /// <summary> </summary>
        public System.Data.Linq.Table<User> Users {
            get {
                return this.GetTable<User>();
            }
        }

        /// <summary> </summary>
        public System.Data.Linq.Table<Client> Clients {
            get {
                return this.GetTable<Client>();
            }
        }

        /// <summary> </summary>
        public System.Data.Linq.Table<ClientAuthorization> ClientAuthorizations {
            get {
                return this.GetTable<ClientAuthorization>();
            }
        }

        /// <summary> </summary>
        public System.Data.Linq.Table<Nonce> Nonces {
            get {
                return this.GetTable<Nonce>();
            }
        }

        /// <summary> </summary>
        public System.Data.Linq.Table<SymmetricCryptoKey> SymmetricCryptoKeys {
            get {
                return this.GetTable<SymmetricCryptoKey>();
            }
        }
    }

    /// <summary> </summary>
    [global::System.Data.Linq.Mapping.TableAttribute(Name = "dbo.[User]")]
    public partial class User : INotifyPropertyChanging, INotifyPropertyChanged {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(string.Empty);

        private int _UserId;

        private string _OpenIDClaimedIdentifier;

        private string _OpenIDFriendlyIdentifier;

        private EntitySet<ClientAuthorization> _OAuthTokens;

        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnUserIdChanging(int value);
        partial void OnUserIdChanged();
        partial void OnOpenIDClaimedIdentifierChanging(string value);
        partial void OnOpenIDClaimedIdentifierChanged();
        partial void OnOpenIDFriendlyIdentifierChanging(string value);
        partial void OnOpenIDFriendlyIdentifierChanged();
        #endregion

        /// <summary> </summary>
        public User() {
            this._OAuthTokens = new EntitySet<ClientAuthorization>(new Action<ClientAuthorization>(this.attach_OAuthTokens), new Action<ClientAuthorization>(this.detach_OAuthTokens));
            OnCreated();
        }

        /// <summary> </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UserId", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int UserId {
            get {
                return this._UserId;
            }
            set {
                if ((this._UserId != value)) {
                    this.OnUserIdChanging(value);
                    this.SendPropertyChanging();
                    this._UserId = value;
                    this.SendPropertyChanged("UserId");
                    this.OnUserIdChanged();
                }
            }
        }

        /// <summary> </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_OpenIDClaimedIdentifier", DbType = "NVarChar(150) NOT NULL", CanBeNull = false)]
        public string OpenIDClaimedIdentifier {
            get {
                return this._OpenIDClaimedIdentifier;
            }
            set {
                if ((this._OpenIDClaimedIdentifier != value)) {
                    this.OnOpenIDClaimedIdentifierChanging(value);
                    this.SendPropertyChanging();
                    this._OpenIDClaimedIdentifier = value;
                    this.SendPropertyChanged("OpenIDClaimedIdentifier");
                    this.OnOpenIDClaimedIdentifierChanged();
                }
            }
        }

        /// <summary> </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_OpenIDFriendlyIdentifier", DbType = "NVarChar(150)")]
        public string OpenIDFriendlyIdentifier {
            get {
                return this._OpenIDFriendlyIdentifier;
            }
            set {
                if ((this._OpenIDFriendlyIdentifier != value)) {
                    this.OnOpenIDFriendlyIdentifierChanging(value);
                    this.SendPropertyChanging();
                    this._OpenIDFriendlyIdentifier = value;
                    this.SendPropertyChanged("OpenIDFriendlyIdentifier");
                    this.OnOpenIDFriendlyIdentifierChanged();
                }
            }
        }

        /// <summary> </summary>
        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "User_ClientAuthorization", Storage = "_OAuthTokens", ThisKey = "UserId", OtherKey = "UserId")]
        public EntitySet<ClientAuthorization> ClientAuthorizations {
            get {
                return this._OAuthTokens;
            }
            set {
                this._OAuthTokens.Assign(value);
            }
        }

        /// <summary> </summary>
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary> </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary> </summary>
        protected virtual void SendPropertyChanging() {
            if ((this.PropertyChanging != null)) {
                this.PropertyChanging(this, emptyChangingEventArgs);
            }
        }

        /// <summary> </summary>
        protected virtual void SendPropertyChanged(String propertyName) {
            if ((this.PropertyChanged != null)) {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary> </summary>
        private void attach_OAuthTokens(ClientAuthorization entity) {
            this.SendPropertyChanging();
            entity.User = this;
        }

        /// <summary> </summary>
        private void detach_OAuthTokens(ClientAuthorization entity) {
            this.SendPropertyChanging();
            entity.User = null;
        }
    }

    /// <summary> </summary>
    [global::System.Data.Linq.Mapping.TableAttribute(Name = "dbo.Client")]
    public partial class Client : INotifyPropertyChanging, INotifyPropertyChanged {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(string.Empty);

        private int _ClientId;

        private string _ClientIdentifier;

        private string _ClientSecret;

        private string _Callback;

        private string _Name;

        private int _ClientType;

        private EntitySet<ClientAuthorization> _OAuthTokens;

        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnClientIdChanging(int value);
        partial void OnClientIdChanged();
        partial void OnClientIdentifierChanging(string value);
        partial void OnClientIdentifierChanged();
        partial void OnClientSecretChanging(string value);
        partial void OnClientSecretChanged();
        partial void OnCallbackChanging(string value);
        partial void OnCallbackChanged();
        partial void OnNameChanging(string value);
        partial void OnNameChanged();
        partial void OnClientTypeChanging(int value);
        partial void OnClientTypeChanged();
        #endregion

        /// <summary> </summary>
        public Client() {
            this._OAuthTokens = new EntitySet<ClientAuthorization>(new Action<ClientAuthorization>(this.attach_OAuthTokens), new Action<ClientAuthorization>(this.detach_OAuthTokens));
            OnCreated();
        }

        /// <summary> </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ClientId", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int ClientId {
            get {
                return this._ClientId;
            }
            set {
                if ((this._ClientId != value)) {
                    this.OnClientIdChanging(value);
                    this.SendPropertyChanging();
                    this._ClientId = value;
                    this.SendPropertyChanged("ClientId");
                    this.OnClientIdChanged();
                }
            }
        }

        /// <summary> </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ClientIdentifier", DbType = "NVarChar(50) NOT NULL", CanBeNull = false)]
        public string ClientIdentifier {
            get {
                return this._ClientIdentifier;
            }
            set {
                if ((this._ClientIdentifier != value)) {
                    this.OnClientIdentifierChanging(value);
                    this.SendPropertyChanging();
                    this._ClientIdentifier = value;
                    this.SendPropertyChanged("ClientIdentifier");
                    this.OnClientIdentifierChanged();
                }
            }
        }

        /// <summary> </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ClientSecret", DbType = "NVarChar(50)")]
        public string ClientSecret {
            get {
                return this._ClientSecret;
            }
            set {
                if ((this._ClientSecret != value)) {
                    this.OnClientSecretChanging(value);
                    this.SendPropertyChanging();
                    this._ClientSecret = value;
                    this.SendPropertyChanged("ClientSecret");
                    this.OnClientSecretChanged();
                }
            }
        }

        /// <summary> </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Callback")]
        public string Callback {
            get {
                return this._Callback;
            }
            set {
                if ((this._Callback != value)) {
                    this.OnCallbackChanging(value);
                    this.SendPropertyChanging();
                    this._Callback = value;
                    this.SendPropertyChanged("Callback");
                    this.OnCallbackChanged();
                }
            }
        }

        /// <summary> </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Name", CanBeNull = false)]
        public string Name {
            get {
                return this._Name;
            }
            set {
                if ((this._Name != value)) {
                    this.OnNameChanging(value);
                    this.SendPropertyChanging();
                    this._Name = value;
                    this.SendPropertyChanged("Name");
                    this.OnNameChanged();
                }
            }
        }

        /// <summary> </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ClientType")]
        public int ClientType {
            get {
                return this._ClientType;
            }
            set {
                if ((this._ClientType != value)) {
                    this.OnClientTypeChanging(value);
                    this.SendPropertyChanging();
                    this._ClientType = value;
                    this.SendPropertyChanged("ClientType");
                    this.OnClientTypeChanged();
                }
            }
        }

        /// <summary> </summary>
        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "Client_ClientAuthorization", Storage = "_OAuthTokens", ThisKey = "ClientId", OtherKey = "ClientId")]
        public EntitySet<ClientAuthorization> ClientAuthorizations {
            get {
                return this._OAuthTokens;
            }
            set {
                this._OAuthTokens.Assign(value);
            }
        }

        /// <summary> </summary>
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary> </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary> </summary>
        protected virtual void SendPropertyChanging() {
            if ((this.PropertyChanging != null)) {
                this.PropertyChanging(this, emptyChangingEventArgs);
            }
        }

        /// <summary> </summary>
        protected virtual void SendPropertyChanged(String propertyName) {
            if ((this.PropertyChanged != null)) {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary> </summary>
        private void attach_OAuthTokens(ClientAuthorization entity) {
            this.SendPropertyChanging();
            entity.Client = this;
        }

        /// <summary> </summary>
        private void detach_OAuthTokens(ClientAuthorization entity) {
            this.SendPropertyChanging();
            entity.Client = null;
        }
    }

    /// <summary> </summary>
    [global::System.Data.Linq.Mapping.TableAttribute(Name = "dbo.ClientAuthorization")]
    public partial class ClientAuthorization : INotifyPropertyChanging, INotifyPropertyChanged {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(string.Empty);

        private int _AuthorizationId;

        private System.DateTime _IssueDate;

        private int _ClientId;

        private int _UserId;

        private string _Scope;

        private System.Nullable<System.DateTime> _ExpirationDateUtc;

        private EntityRef<Client> _Client;

        private EntityRef<User> _User;

        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnAuthorizationIdChanging(int value);
        partial void OnAuthorizationIdChanged();
        partial void OnCreatedOnUtcChanging(System.DateTime value);
        partial void OnCreatedOnUtcChanged();
        partial void OnClientIdChanging(int value);
        partial void OnClientIdChanged();
        partial void OnUserIdChanging(int value);
        partial void OnUserIdChanged();
        partial void OnScopeChanging(string value);
        partial void OnScopeChanged();
        partial void OnExpirationDateUtcChanging(System.Nullable<System.DateTime> value);
        partial void OnExpirationDateUtcChanged();
        #endregion

        /// <summary> </summary>
        public ClientAuthorization() {
            this._Client = default(EntityRef<Client>);
            this._User = default(EntityRef<User>);
            OnCreated();
        }

        /// <summary> </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_AuthorizationId", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int AuthorizationId {
            get {
                return this._AuthorizationId;
            }
            set {
                if ((this._AuthorizationId != value)) {
                    this.OnAuthorizationIdChanging(value);
                    this.SendPropertyChanging();
                    this._AuthorizationId = value;
                    this.SendPropertyChanged("AuthorizationId");
                    this.OnAuthorizationIdChanged();
                }
            }
        }

        /// <summary> </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_IssueDate", DbType = "DateTime NOT NULL")]
        public System.DateTime CreatedOnUtc {
            get {
                return this._IssueDate;
            }
            set {
                if ((this._IssueDate != value)) {
                    this.OnCreatedOnUtcChanging(value);
                    this.SendPropertyChanging();
                    this._IssueDate = value;
                    this.SendPropertyChanged("CreatedOnUtc");
                    this.OnCreatedOnUtcChanged();
                }
            }
        }

        /// <summary> </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ClientId", DbType = "Int NOT NULL")]
        public int ClientId {
            get {
                return this._ClientId;
            }
            set {
                if ((this._ClientId != value)) {
                    if (this._Client.HasLoadedOrAssignedValue) {
                        throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this.OnClientIdChanging(value);
                    this.SendPropertyChanging();
                    this._ClientId = value;
                    this.SendPropertyChanged("ClientId");
                    this.OnClientIdChanged();
                }
            }
        }

        /// <summary> </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UserId", DbType = "Int")]
        public int UserId {
            get {
                return this._UserId;
            }
            set {
                if ((this._UserId != value)) {
                    if (this._User.HasLoadedOrAssignedValue) {
                        throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this.OnUserIdChanging(value);
                    this.SendPropertyChanging();
                    this._UserId = value;
                    this.SendPropertyChanged("UserId");
                    this.OnUserIdChanged();
                }
            }
        }

        /// <summary> </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Scope", DbType = "nvarchar(MAX)", CanBeNull = false)]
        public string Scope {
            get {
                return this._Scope;
            }
            set {
                if ((this._Scope != value)) {
                    this.OnScopeChanging(value);
                    this.SendPropertyChanging();
                    this._Scope = value;
                    this.SendPropertyChanged("Scope");
                    this.OnScopeChanged();
                }
            }
        }

        /// <summary> </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ExpirationDateUtc", DbType = "DateTime NULL")]
        public System.Nullable<System.DateTime> ExpirationDateUtc {
            get {
                return this._ExpirationDateUtc;
            }
            set {
                if ((this._ExpirationDateUtc != value)) {
                    this.OnExpirationDateUtcChanging(value);
                    this.SendPropertyChanging();
                    this._ExpirationDateUtc = value;
                    this.SendPropertyChanged("ExpirationDateUtc");
                    this.OnExpirationDateUtcChanged();
                }
            }
        }

        /// <summary> </summary>
        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "Client_ClientAuthorization", Storage = "_Client", ThisKey = "ClientId", OtherKey = "ClientId", IsForeignKey = true, DeleteOnNull = true, DeleteRule = "CASCADE")]
        public Client Client {
            get {
                return this._Client.Entity;
            }
            set {
                Client previousValue = this._Client.Entity;
                if (((previousValue != value)
                            || (this._Client.HasLoadedOrAssignedValue == false))) {
                    this.SendPropertyChanging();
                    if ((previousValue != null)) {
                        this._Client.Entity = null;
                        previousValue.ClientAuthorizations.Remove(this);
                    }
                    this._Client.Entity = value;
                    if ((value != null)) {
                        value.ClientAuthorizations.Add(this);
                        this._ClientId = value.ClientId;
                    } else {
                        this._ClientId = default(int);
                    }
                    this.SendPropertyChanged("Client");
                }
            }
        }

        /// <summary> </summary>
        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "User_ClientAuthorization", Storage = "_User", ThisKey = "UserId", OtherKey = "UserId", IsForeignKey = true, DeleteRule = "CASCADE")]
        public User User {
            get {
                return this._User.Entity;
            }
            set {
                User previousValue = this._User.Entity;
                if (((previousValue != value)
                            || (this._User.HasLoadedOrAssignedValue == false))) {
                    this.SendPropertyChanging();
                    if ((previousValue != null)) {
                        this._User.Entity = null;
                        previousValue.ClientAuthorizations.Remove(this);
                    }
                    this._User.Entity = value;
                    if ((value != null)) {
                        value.ClientAuthorizations.Add(this);
                        this._UserId = value.UserId;
                    } else {
                        this._UserId = default(int);
                    }
                    this.SendPropertyChanged("User");
                }
            }
        }

        /// <summary> </summary>
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary> </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary> </summary>
        protected virtual void SendPropertyChanging() {
            if ((this.PropertyChanging != null)) {
                this.PropertyChanging(this, emptyChangingEventArgs);
            }
        }

        /// <summary> </summary>
        protected virtual void SendPropertyChanged(String propertyName) {
            if ((this.PropertyChanged != null)) {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <summary> </summary>
    [global::System.Data.Linq.Mapping.TableAttribute(Name = "dbo.Nonce")]
    public partial class Nonce : INotifyPropertyChanging, INotifyPropertyChanged {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(string.Empty);

        private string _Context;

        private string _Code;

        private System.DateTime _Timestamp;

        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnContextChanging(string value);
        partial void OnContextChanged();
        partial void OnCodeChanging(string value);
        partial void OnCodeChanged();
        partial void OnTimestampChanging(System.DateTime value);
        partial void OnTimestampChanged();
        #endregion

        /// <summary> </summary>
        public Nonce() {
            OnCreated();
        }

        /// <summary> </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Context", CanBeNull = false, IsPrimaryKey = true)]
        public string Context {
            get {
                return this._Context;
            }
            set {
                if ((this._Context != value)) {
                    this.OnContextChanging(value);
                    this.SendPropertyChanging();
                    this._Context = value;
                    this.SendPropertyChanged("Context");
                    this.OnContextChanged();
                }
            }
        }

        /// <summary> </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Code", CanBeNull = false, IsPrimaryKey = true)]
        public string Code {
            get {
                return this._Code;
            }
            set {
                if ((this._Code != value)) {
                    this.OnCodeChanging(value);
                    this.SendPropertyChanging();
                    this._Code = value;
                    this.SendPropertyChanged("Code");
                    this.OnCodeChanged();
                }
            }
        }

        /// <summary> </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Timestamp", IsPrimaryKey = true)]
        public System.DateTime Timestamp {
            get {
                return this._Timestamp;
            }
            set {
                if ((this._Timestamp != value)) {
                    this.OnTimestampChanging(value);
                    this.SendPropertyChanging();
                    this._Timestamp = value;
                    this.SendPropertyChanged("Timestamp");
                    this.OnTimestampChanged();
                }
            }
        }

        /// <summary> </summary>
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary> </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary> </summary>
        protected virtual void SendPropertyChanging() {
            if ((this.PropertyChanging != null)) {
                this.PropertyChanging(this, emptyChangingEventArgs);
            }
        }

        /// <summary> </summary>
        protected virtual void SendPropertyChanged(String propertyName) {
            if ((this.PropertyChanged != null)) {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <summary> </summary>
    [global::System.Data.Linq.Mapping.TableAttribute(Name = "")]
    public partial class SymmetricCryptoKey : INotifyPropertyChanging, INotifyPropertyChanged {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(string.Empty);

        private string _Bucket;

        private string _Handle;

        private System.DateTime _ExpiresUtc;

        private byte[] _Secret;

        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnBucketChanging(string value);
        partial void OnBucketChanged();
        partial void OnHandleChanging(string value);
        partial void OnHandleChanged();
        partial void OnExpiresUtcChanging(System.DateTime value);
        partial void OnExpiresUtcChanged();
        partial void OnSecretChanging(byte[] value);
        partial void OnSecretChanged();
        #endregion

        /// <summary> </summary>
        public SymmetricCryptoKey() {
            OnCreated();
        }

        /// <summary> </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Bucket", CanBeNull = false, IsPrimaryKey = true, UpdateCheck = UpdateCheck.Never)]
        public string Bucket {
            get {
                return this._Bucket;
            }
            set {
                if ((this._Bucket != value)) {
                    this.OnBucketChanging(value);
                    this.SendPropertyChanging();
                    this._Bucket = value;
                    this.SendPropertyChanged("Bucket");
                    this.OnBucketChanged();
                }
            }
        }

        /// <summary> </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Handle", CanBeNull = false, IsPrimaryKey = true, UpdateCheck = UpdateCheck.Never)]
        public string Handle {
            get {
                return this._Handle;
            }
            set {
                if ((this._Handle != value)) {
                    this.OnHandleChanging(value);
                    this.SendPropertyChanging();
                    this._Handle = value;
                    this.SendPropertyChanged("Handle");
                    this.OnHandleChanged();
                }
            }
        }

        /// <summary> </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ExpiresUtc", UpdateCheck = UpdateCheck.Never)]
        public System.DateTime ExpiresUtc {
            get {
                return this._ExpiresUtc;
            }
            set {
                if ((this._ExpiresUtc != value)) {
                    this.OnExpiresUtcChanging(value);
                    this.SendPropertyChanging();
                    this._ExpiresUtc = value;
                    this.SendPropertyChanged("ExpiresUtc");
                    this.OnExpiresUtcChanged();
                }
            }
        }

        /// <summary> </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Secret", CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
        public byte[] Secret {
            get {
                return this._Secret;
            }
            set {
                if ((this._Secret != value)) {
                    this.OnSecretChanging(value);
                    this.SendPropertyChanging();
                    this._Secret = value;
                    this.SendPropertyChanged("Secret");
                    this.OnSecretChanged();
                }
            }
        }

        /// <summary> </summary>
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary> </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary> </summary>
        protected virtual void SendPropertyChanging() {
            if ((this.PropertyChanging != null)) {
                this.PropertyChanging(this, emptyChangingEventArgs);
            }
        }

        /// <summary> </summary>
        protected virtual void SendPropertyChanged(String propertyName) {
            if ((this.PropertyChanged != null)) {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
