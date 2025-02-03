Ask for XML (IMPORTANT: Do NOT use listing tags right now in Co-Pilot!)
==========================================================================
Can you please add XML Comments to this class? 
Please also try to describe as best as possible the respective functionality in the remarks section, 
have the remarks/paragraph tags always isolated (newline, paragraph tag, newline) and use paragraph tags inside the remarks.

IMPORTANT: Please indent with one space to structure the XML, like:

/// <summary>
///  Summary text.
/// </summary>
/// <remarks>
///  <para>
///   This is a sample paragraph to guide an LLM to the structuring of docu-tags. We want
///   to make sure, the the XML comments remain human readable, and Jeremy remains happy.
///   To keep me happy, I humbly ask for lines no longer than around 90-100 characters.
///  </para>
///  <para>
///   It also handles the processing of paragraphs within a conversation, identifying and managing code listings.
///  </para>
/// </remarks>



Ask for Controls in Table Layout Panels (Sample):
=================================================
* Can you add a Radio Buttons to the TableLayoutPanel rows named "Extract code in Markdown code blocks automatically." 
  - Put this in a Panel, before you add it to the TableLayoutPanel since we want to add a checkbox "Automatically save the extracted listing/text files to the Conversation folder". 
  - This checkbox should only be enabled, when the option is selected. 
  - Add another checkbox which says "Store extracted conversation files in dedicated folders deren Name vom automatischen Titel der Konversation abgeleitet ist."

* Add another row with the Option "Select a folder for text-file/data extraction, only at the time when the Conversation es erfordert".
* Store the extracted files automatically in MyDocuments under "\Chatty".
  - Also here, add the checkbox (and so we need to add a Panel to the Layout Row) which says "Store extracted conversation files in dedicated folders deren Name vom automatischen Titel der Konversation abgeleitet ist."
In this prompt, I had a few typos and types things in German, because I could remember the english phrases. Please make sure, the captions of the  Radio- and CheckBox Buttons are verstaendlich und grammatikalisch korrekt.
