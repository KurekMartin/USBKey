using Usb.Events;

class Program
{
    static void Main(string[] args)
    {
        using IUsbEventWatcher usbEventWatcher = new UsbEventWatcher();

        usbEventWatcher.UsbDriveMounted += (_, path) =>
        {
            Console.WriteLine(path);
        };
        Console.WriteLine("Insert USB");
        while (true) { };
    }
}