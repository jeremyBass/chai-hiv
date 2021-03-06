using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Castle.ActiveRecord;
using NHibernate.Criterion;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using log4net;
using System.IO;

namespace stellar.Models {

    /// <summary> </summary>
    public class site_config : ConfigurationSection {

    }

    /// <summary> </summary>
    [ActiveRecord(Lazy = true, BatchSize = 30)]
    public class site : ActiveRecordBase<site> {
        ILog log = log4net.LogManager.GetLogger("Site");
        //SiteService siteService = new SiteService();
        //Resource _RootResource;

        /// <summary> </summary>
        [PrimaryKey("site_id")]
        virtual public int id { get; set; }

        /// <summary> </summary>
        [Property]
        virtual public String name { get; set; }

        /// <summary> </summary>
        [Property]
        virtual public String alias { get; set; }

        /// <summary> </summary>
        [Property(SqlType = "nvarchar(MAX)")]
        virtual public String siteroot { get; set; }

        /// <summary> </summary>
        [Property(SqlType = "nvarchar(MAX)")]
        virtual public String base_url { get; set; }

        /// <summary> </summary>
        [Property(SqlType = "nvarchar(MAX)")]
        virtual public string local_path { get; set; }

        /// <summary> </summary>
        [Property(SqlType = "nvarchar(MAX)")]
        virtual public String static_directories { get; set; }

        /// <summary> </summary>
        [Property]
        virtual public bool static_rendering { get; set; }

        /// <summary> </summary>
        [Property]
        virtual public bool is_default { get; set; }


        /// <summary> </summary>
        [HasMany(typeof(_base), Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan)]
        virtual public IList<_base> items { get; set; }

        /// <summary> </summary>
        [HasMany(typeof(connections_master), Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan)]
        virtual public IList<connections_master> slave_to { get; set; }

        /// <summary> </summary>
        [HasMany(typeof(connections_slave), Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan)]
        virtual public IList<connections_slave> master_of { get; set; }

        /// <summary> </summary>
        [HasMany(typeof(options), Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan)]
        virtual public IList<options> options { get; set; }

        /// <summary> </summary>
        virtual public String get_option(String key) {
            String result = "";
            if (this.id!=0) {
                options data = ActiveRecordBase<options>.FindOne(
                       new List<AbstractCriterion>() { 
                       Expression.Eq("site", this), 
                       Expression.Eq("option_key", key)
                   }.ToArray()
                   );

                if (data != null)
                    result = data.value;
            }
            return result;
        }

      /*  [HasAndBelongsToMany(typeof(campus), Lazy = true, Table = "site_to_campus", ColumnKey = "site_id", ColumnRef = "campus_id", NotFoundBehaviour = NotFoundBehaviour.Ignore)]
        virtual public IList<campus> campus { get; set; }
        */
    }

    /// <summary> </summary>
    [ActiveRecord(BatchSize = 30)]
    public class options : ActiveRecordBase<options> {

        /// <summary> </summary>
        [PrimaryKey("option_id")]
        virtual public int id { get; set; }

        /// <summary> </summary>
        [Property(SqlType = "nvarchar(MAX)")]
        virtual public String value { get; set; }

        /// <summary> </summary>
        [Property]
        virtual public String option_key { get; set; }

        /// <summary> </summary>
        [Property]
        virtual public bool is_overwritable { get; set; }

        /// <summary> </summary>
        [BelongsTo]
        virtual public site site { get; set; }
    }

    /* this may not be the best way .. come back to */
    /// <summary> </summary>
    [ActiveRecord(BatchSize = 30)]
    public class module : _base {

        /// <summary> </summary>
        [JoinedKey("module_id")]
        virtual public int id { get; set; }

        /// <summary> </summary>
        [Property(SqlType = "nvarchar(MAX)")]
        virtual public String notes { get; set; }

        /*[BelongsTo]
        virtual public site site { get; set; }*/

        /// <summary> </summary>
        [Property]
        virtual public bool installed { get; set; }

    }


}
