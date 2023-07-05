using System.Collections;
using System.Collections.Generic;
public interface Subject {

    public void AddObserver(Observer ops);
    public void RemoveObserver(Observer ops);
    public void NotifyToObserver();

}

public interface Observer {
    public void Notified();

}