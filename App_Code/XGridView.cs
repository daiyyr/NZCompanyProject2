using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
public class XGridView
{
    public static void BindData(GridView gv, IList i)
    {
        gv.DataSource = null;
        gv.DataBind();
        gv.DataSource = i;
        gv.DataBind();
        if (gv.Rows.Count > 0)
        {
            gv.HeaderRow.TableSection = TableRowSection.TableHeader;
            gv.FooterRow.TableSection = TableRowSection.TableFooter;
        }
    }
    public static void BindData(GridView gv, DataTable i)
    {
        gv.DataSource = null;
        gv.DataBind();
        if (i.Rows.Count > 0)
        {
            if (i.Rows.Count > 0)
            {
                gv.DataSource = i;

                gv.DataBind();
                if (gv.Rows.Count > 0)
                {
                    gv.HeaderRow.TableSection = TableRowSection.TableHeader;
                    gv.FooterRow.TableSection = TableRowSection.TableFooter;
                }
            }
        }
        else
        {
            i.Rows.Add(i.NewRow());
            if (i.Rows.Count > 0)
            {
                gv.DataSource = i;

                gv.DataBind();
                if (gv.Rows.Count > 0)
                {
                    gv.HeaderRow.TableSection = TableRowSection.TableHeader;
                    gv.FooterRow.TableSection = TableRowSection.TableFooter;
                }
            }
            gv.Rows[0].Visible = false;
        }
    }
    public static void AddHead(GridView gv)
    {
        if (gv.Rows.Count > 0)
        {
            gv.HeaderRow.TableSection = TableRowSection.TableHeader;
            gv.FooterRow.TableSection = TableRowSection.TableFooter;
        }
    }
}
