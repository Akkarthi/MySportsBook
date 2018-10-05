package md5cd696db7dbef1e47e5beb4afb6d3b926;


public class Helper
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("MySportsBook.Helper, MySportsBook", Helper.class, __md_methods);
	}


	public Helper ()
	{
		super ();
		if (getClass () == Helper.class)
			mono.android.TypeManager.Activate ("MySportsBook.Helper, MySportsBook", "", this, new java.lang.Object[] {  });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
