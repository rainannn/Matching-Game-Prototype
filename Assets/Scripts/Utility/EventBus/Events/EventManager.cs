using System;

public static class EventManager
{
    private static readonly EventBus EventBus;

    static EventManager()
    {
        Container.Initialize();
        EventBus = ServiceLocator.Instance.Resolve<EventBus>();
    }
    
    public static void Subscribe<T>(Action<T> action) where T : IEvent
    {
        EventBus.Subscribe(action);
    }

    
    public static void Unsubscribe<T>(Action<T> action) where T : IEvent
    {
        EventBus.Unsubscribe(action);
    } 
    
   
    public static void Fire<T>(T payload) where T : IEvent
    {
        EventBus.Fire(payload);
    }
}