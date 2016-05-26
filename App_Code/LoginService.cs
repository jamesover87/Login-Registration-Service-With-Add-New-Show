using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "LoginService" in code, svc and config file together.
public class LoginService : ILoginService
{
    ShowTrackerEntities db = new ShowTrackerEntities();

    public bool AddArtist(Artist a)
    {
        Artist artist = new Artist();
        artist.ArtistName = a.ArtistName;
        bool result = true;
        try
        {
            db.Artists.Add(artist);
            db.SaveChanges();
        }
        catch
        {
            result = false;
        }
        return result;
    }

    public bool AddShow(Show s, ShowDetail sd)
    {
        Show show = new Show();
        ShowDetail showDetail = new ShowDetail();
        Boolean result = true;

        show.ShowName = s.ShowName;
        show.ShowDateEntered = DateTime.Now;
        show.ShowDate = s.ShowDate;
        show.ShowTime = s.ShowTime;
        show.ShowTicketInfo = s.ShowTicketInfo;
        show.VenueKey = s.VenueKey;

        showDetail.Show = show;
        showDetail.ShowDetailArtistStartTime = sd.ShowDetailArtistStartTime;
        showDetail.ShowDetailAdditional = sd.ShowDetailAdditional;
        try {
            db.Shows.Add(show);
            db.ShowDetails.Add(showDetail);
            db.SaveChanges();
        }
        catch
        {
            result = false;
        }

        return result;

    }

    public bool RegisterVenue(Venue v, VenueLogin vl)
    {
        bool result = true;
        int pass = db.usp_RegisterVenue(v.VenueName, v.VenueAddress, v.VenueCity, v.VenueState, v.VenueZipCode, v.VenuePhone, v.VenueEmail, v.VenueWebPage, v.VenueAgeRestriction, vl.VenueLoginUserName, vl.VenueLoginPasswordPlain);
        if (pass == -1)
        {
            result = false;
        }
        return result;
    }

    public int VenueLogin(string userName, string password)
    {
        int venueKey = 0;
        int result = db.usp_venueLogin(userName, password);
        if (result != -1)
        {
            var key = (from vl in db.VenueLogins
                       where vl.VenueLoginUserName.Equals(userName)
                       select new { vl.VenueKey }).FirstOrDefault();
            venueKey = (int) key.VenueKey;
        }
        return venueKey;
    }
}
