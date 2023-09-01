using System;
using MUD;
using System.Data.SQLite;
using System.Diagnostics;
using System.Numerics;
using MUD.SQL;

internal class Program
{
    static void Main(string[] args)
    {
        Database.instance = new Database();

        Area area = new Area("Soulreaver Keep", 13, 13);
        Dictionary<int, string> courtyardDecay = new Dictionary<int, string>();
        courtyardDecay[0] = "The grass is stamped into the ground, and you feel a creeping presence throughout the area.";
        string desc = "The courtyard is somewhat overgrown, with few signs of life. " +
            "           It seems as though it's been a while since anyone had walked through here.";
        courtyardDecay[3600] = FormatMultilineString(desc);
        desc = "The courtyard is completely overgrown, with no signs of life. " +
            "   How long has it been since anyone had walked through here?";
        courtyardDecay[36000] = FormatMultilineString(desc);

        Dictionary<int, string> noDecay = new Dictionary<int, string>();
        courtyardDecay[0] = "";

        string[] CourtyardDesc = new string[5];
        string[] CourtyardBattlementDesc = new string[2];
        desc = @"A chilling breeze dances through the air, carrying with it an eerie sense of history and sorrow.
                The once-grand courtyard is now a haunting reminder of the castle's past, overrun by encroaching vines
                and crumbling statues that have weathered the passage of time.";
        CourtyardDesc[0] = FormatMultilineString(desc);
        desc = @"The cobblestone paths leading away from the fountain are cracked and uneven, 
                 a testament to the neglect that has befallen this place. As you walk along the pathways, 
                 the distant echoes of your footsteps mix with the haunting sounds of owls hooting and the 
                 wind rustling through the branches of nearby trees.";
        CourtyardDesc[1] = FormatMultilineString(desc);
        desc = @"The courtyard lies in ruin, its once grand architecture now shrouded in neglect and overgrown 
                 with thorny vegetation. A cold breeze carries eerie whispers, and skeletal figures roam the grounds, 
                 adding to the haunted ambiance. The air is heavy with the sense of foreboding, warning any who dare 
                 to enter the castle's depths of the perils that await.";
        CourtyardDesc[2] = FormatMultilineString(desc);
        desc = @"The courtyard stands in eerie stillness, a haunting realm of neglect and decay. Overgrown vegetation 
                 entwines the weathered stone walls. Shadows dance amidst the whispers, hinting at the undead presence 
                 that infests this once grand space.";
        CourtyardDesc[3] = FormatMultilineString(desc);
        desc = @"In the courtyard, vines ensnare crumbling walls, while an abandoned fountain stands as a silent witness 
                 to past grandeur. The air carries whispers of long-gone souls, and skeletal figures lurk in the shadows, 
                 warning of the horrors within the castle.";
        CourtyardDesc[4] = FormatMultilineString(desc);
        desc = @"As you approach the courtyard near the castle's battlements, a commanding view of the surrounding lands unfolds 
                 before you. The stone walls of the battlements stand tall and weathered, offering both a sense of protection and 
                 an air of ancient history. You can see the remnants of old archer slits, reminding you of the castle's past.";
        CourtyardBattlementDesc[0] = FormatMultilineString(desc);
        desc = @"The courtyard near the castle's battlements offers a commanding view of the rugged landscape below. Weathered 
                 stone walls stand as witnesses to past conflicts, while wildflowers and gnarled trees persist amidst the barren patches. 
                 The haunting silence and skeletal remnants evoke a mix of awe and trepidation, urging caution as you prepare yourself.";
        CourtyardBattlementDesc[1] = FormatMultilineString(desc);


        desc = @"As you cautiously step into the courtyard, a shiver runs down your spine, sensing an unsettling chill in 
                 the air. The once magnificent space now lies in ruins, with nature reclaiming its territory. Vines and 
                 thorny shrubs claw at the weathered stone walls, and the silence is broken only by distant whispers and 
                 the ominous sounds of skeletal footsteps echoing from within the darkened castle ahead.";
        Room room = new Room(area,
            "The Courtyard",
            FormatMultilineString(desc),
            courtyardDecay);
        area.AddRoom(6, 1, room);

        desc = @"As you walk further into the courtyard, your footsteps echo softly against the worn cobblestones. 
                 The crunch of fallen leaves and the occasional snap of a twig break the otherwise eerie silence 
                 that envelops the place. The overgrown vines seem to reach out for you, adding to the feeling of 
                 being watched by unseen eyes. With every step, the weight of the haunting atmosphere presses down 
                 on your shoulders, yet your curiosity and determination push you forward, deeper into the heart of darkness.";
        room = new Room(area,
            "The Courtyard",
            FormatMultilineString(desc),
            courtyardDecay);
        area.AddRoom(5, 1, room);

        desc = @"As you walk further into the courtyard, your footsteps echo softly against the worn cobblestones. 
                 The crunch of fallen leaves and the occasional snap of a twig break the otherwise eerie silence 
                 that envelops the place. The overgrown vines seem to reach out for you, adding to the feeling of 
                 being watched by unseen eyes. With every step, the weight of the haunting atmosphere presses down 
                 on your shoulders, yet your curiosity and determination push you forward, deeper into the heart of darkness.";
        room = new Room(area,
            "The Courtyard",
            FormatMultilineString(desc),
            courtyardDecay);
        area.AddRoom(7, 1, room);

        desc = @"As you walk toward the courtyard, the imposing gatehouse looms overhead, its dark archway a stark contrast 
                 to the fading daylight outside. You can feel the weight of history in the sturdy stone walls, sensing the 
                 stories of countless defenders who once manned these defenses. As you step into the courtyard, the echo of 
                 your footsteps fills the space, and the chill in the air intensifies, giving you a sense of being watched 
                 from the shadows above.";
        room = new Room(area,
            "The Courtyard",
            FormatMultilineString(desc),
            courtyardDecay);
        area.AddRoom(1, 6, room);

        desc = @"As you walk toward the courtyard, the imposing gatehouse looms overhead, its dark archway a stark contrast 
                 to the fading daylight outside. You can feel the weight of history in the sturdy stone walls, sensing the 
                 stories of countless defenders who once manned these defenses. As you step into the courtyard, the echo of 
                 your footsteps fills the space, and the chill in the air intensifies, giving you a sense of being watched 
                 from the shadows above.";
        room = new Room(area,
            "The Courtyard",
            FormatMultilineString(desc),
            courtyardDecay);
        area.AddRoom(12, 6, room);

        int[][] courtyardRooms = { 
            new int[] { 2, 1 },
            new int[] { 3, 1 },
            new int[] { 4, 1 },
            new int[] { 8, 1 },
            new int[] { 9, 1 },
            new int[] { 10, 1 },
            new int[] { 1, 2 },
            new int[] { 1, 3 },
            new int[] { 1, 4 },
            new int[] { 1, 5 },
            new int[] { 1, 7 },
            new int[] { 1, 8 },
            new int[] { 1, 9 },
            new int[] { 1, 10 },
            new int[] { 1, 11 },
            new int[] { 11, 2 },
            new int[] { 11, 3 },
            new int[] { 11, 4 },
            new int[] { 11, 5 },
            new int[] { 11, 7 },
            new int[] { 11, 8 },
            new int[] { 11, 9 },
            new int[] { 11, 10 },
            new int[] { 11, 11 },
            new int[] { 2, 12 },
            new int[] { 3, 12 },
            new int[] { 4, 12 },
            new int[] { 5, 12 },
            new int[] { 6, 12 },
            new int[] { 7, 12 },
            new int[] { 8, 12 },
            new int[] { 9, 12 },
            new int[] { 10, 12 },
        };

        Random random = new Random();

        foreach (int[] pos in courtyardRooms)
        {
            room = new Room(area,
                "The Courtyard",
                CourtyardDesc[random.Next(0, CourtyardDesc.Length)],
                courtyardDecay);
            area.AddRoom(pos[0], pos[1], room);
        }

        int[][] courtyardBattlementRooms = {
            new int[] { 11, 1 },
            new int[] { 1, 1 },
            new int[] { 1, 12 },
            new int[] { 11, 12 }
        };

        foreach (int[] pos in courtyardBattlementRooms)
        {
            room = new Room(area,
                "The Courtyard",
                CourtyardBattlementDesc[random.Next(0, CourtyardBattlementDesc.Length)],
                courtyardDecay);
            area.AddRoom(pos[0], pos[1], room);
        }

        desc = @"As you approach the gatehouse, the once majestic structure now stands in eerie disarray. Its stone walls 
                 bear the marks of time and neglect, with vines and ivy creeping up the sides, reclaiming what was once 
                 man-made. The heavy wooden doors, once a symbol of protection, now hang slightly ajar, creaking eerily
                 in the wind. Inside, the air is thick with dust and the faint scent of decay, as if it holds secrets of 
                 the castle's haunting past.";
        room = new Room(area,
            "The Gatehouse",
            FormatMultilineString(desc),
            noDecay);
        area.AddRoom(6, 2, room);

        desc = @"As you pass through the gatehouse and step into the main castle area, you find yourself at the entrance to a 
                 long, dimly lit hallway. The once grand corridor now exudes a sense of foreboding, with cracked and faded 
                 tapestries hanging from the stone walls. The flickering torches mounted sporadically along the passage cast 
                 eerie shadows that seem to dance and whisper secrets to one another.";
        room = new Room(area,
            "Grand Hallway",
            FormatMultilineString(desc),
            noDecay);
        area.AddRoom(6, 3, room);

        desc = @"You turn left down the hallway, your steps echoing softly against the ancient stone floor. The air grows colder, 
                 and the dim torchlight becomes scarcer as you venture outwards throughout the castle. Cobwebs, like ghostly veils, 
                 drape the corners, and the walls bear remnants of faded murals, hinting at a forgotten past.";
        room = new Room(area,
            "Grand Hallway",
            FormatMultilineString(desc),
            noDecay);
        area.AddRoom(5, 3, room);

        desc = @"You turn right down the hallway, your steps echoing softly against the ancient stone floor. The air grows colder, 
                 and the dim torchlight becomes scarcer as you venture outwards throughout the castle. Cobwebs, like ghostly veils, 
                 drape the corners, and the walls bear remnants of faded murals, hinting at a forgotten past.";
        room = new Room(area,
            "Grand Hallway",
            FormatMultilineString(desc),
            noDecay);
        area.AddRoom(7, 3, room);

        desc = @"As you near the entrance to the armory, you find the once-sturdy door partially ajar, creaking softly with each gust 
                 of wind. Rusty suits of armor line the walls, some bearing the marks of battles fought long ago. Broken weapons lie 
                 scattered on the floor, remnants of the castle's glorious days of defense. The distant echoes of clinking metal and 
                 faint moans create an unsettling symphony, urging you to steel your nerves before entering the armory's depths.";
        room = new Room(area,
            "Armory Entrance (West)",
            FormatMultilineString(desc),
            noDecay);
        area.AddRoom(4, 3, room);

        desc = @"As you near the entrance to the armory, you find the once-sturdy door partially ajar, creaking softly with each gust 
                 of wind. Rusty suits of armor line the walls, some bearing the marks of battles fought long ago. Broken weapons lie 
                 scattered on the floor, remnants of the castle's glorious days of defense. The distant echoes of clinking metal and 
                 faint moans create an unsettling symphony, urging you to steel your nerves before entering the armory's depths.";
        room = new Room(area,
            "Armory Entrance (East)",
            FormatMultilineString(desc),
            noDecay);
        area.AddRoom(8, 3, room);

        desc = @"Stepping into the armory, you find yourself surrounded by an eerie array of forgotten weaponry and armor. Racks of 
                 rusted swords, axes, and maces line the walls, each blade bearing the scars of battles fought in a bygone era. 
                 Dust-laden shields lean against one another, their once-vibrant colors now muted and faded. In the dim light, 
                 suits of armor stand like silent sentinels, their empty visors seeming to gaze upon intruders with a haunting intensity.";
        room = new Room(area,
            "The Armory (West)",
            FormatMultilineString(desc),
            noDecay);
        area.AddRoom(4, 2, room);

        desc = @"Stepping into the armory, you are greeted by a scene frozen in time. Dusty rays of light filter through narrow slits in 
                 the boarded-up windows, illuminating rows of weaponry neatly organized along the walls. Shelves of rusted swords, pikes, 
                 and maces line one side, while ancient crossbows and arrows rest on racks opposite them. Suits of armor stand ever vigilant, 
                 ready for some unseen threat.";
        room = new Room(area,
            "The Armory (East)",
            FormatMultilineString(desc),
            noDecay);
        area.AddRoom(8, 2, room);

        desc = @"As you ascend a narrow spiral staircase adjacent to the armory, you emerge onto the castle's battlements. The cold wind 
                 whistles through the ancient stone parapets, carrying with it the haunting echoes of battles long past. The battlements 
                 offer a panoramic view of the surrounding desolate landscape, with the castle's decaying turrets and broken ramparts 
                 stretching out before you.";
        room = new Room(area,
            "Battlements (West)",
            FormatMultilineString(desc),
            noDecay);
        area.AddRoom(3, 2, room);

        desc = @"Ascending a narrow spiral staircase from the armory, you emerge onto the weathered battlements that encircle the castle's 
                 upper reaches. The stone parapets bear the scars of time and conflict, with crumbling portions revealing the resilience of 
                 the castle's past defenders. Moss and ivy cling to the ancient stones, adding an air of desolation to the once bustling 
                 defensive line.";
        room = new Room(area,
            "Battlements (East)",
            FormatMultilineString(desc),
            noDecay);
        area.AddRoom(9, 2, room);

        desc = @"As you traverse deeper into the castle, the silence becomes all-encompassing, broken only by the occasional creak of the 
                 floorboards or distant moans echoing through the corridors. The air is heavy with the scent of aged wood and dust, evoking 
                 a sense of time standing still. The hallway is flanked on both sides by ornate gilded frames, each housing a collection of 
                 hauntingly beautiful but faded portraits, their eyes following your every step with an eerie intensity.";
        room = new Room(area,
            "Grand Hallway",
            FormatMultilineString(desc),
            noDecay);
        area.AddRoom(6, 4, room);

        desc = @"As you venture deeper into the castle, the hallway stretches like a forgotten path, its walls adorned with time-worn tapestries 
                 depicting forgotten battles and ancient legends, their vibrant colors now muted by the passage of time. Flickering torches cast 
                 dancing shadows along the stone, whispering secrets in hushed tones as you pass.";
        room = new Room(area,
            "Grand Hallway",
            FormatMultilineString(desc),
            noDecay);
        area.AddRoom(6, 5, room);

        desc = @"The library welcomes you with its enchanting aura, bookshelves stretching tall, cradling leather-bound tomes of various sizes. 
                 Sunlight pours through stained glass windows, painting colorful patterns on the floor where dust motes dance. The air carries 
                 the scent of aged parchment and ink, and the soft rustling of pages whispers like a mesmerizing melody. Comfortable reading
                 nooks beckon, inviting you to lose yourself in the profound wisdom and forgotten tales held within the treasured sanctuary of 
                 knowledge.";
        room = new Room(area,
            "Grand Hallway",
            FormatMultilineString(desc),
            noDecay);
        area.AddRoom(5, 5, room);

        desc = @"As you open the creaking door, the musty odor of neglect fills the air within the storage room. Dim light filters through a 
                 dirty window, barely illuminating the cluttered space. Dust-covered crates, old furniture, and various discarded items 
                 create an obstacle course of forgotten relics. Cobwebs stretch across the corners, hinting at the passage of time within 
                 this long-neglected chamber.";
        room = new Room(area,
            "Grand Hallway",
            FormatMultilineString(desc),
            noDecay);
        area.AddRoom(7, 5, room);

        desc = @"As you approach the hallway leading towards the center of the castle, a palpable sense of magic fills the air. 
                 Soft, glowing orbs of light hover along the walls, illuminating the way with a warm and inviting radiance. 
                 The stone floor beneath your feet feels smooth and slightly warm to the touch, as if imbued with ancient 
                 energies.";
        room = new Room(area,
            "The Castle Heart",
            FormatMultilineString(desc),
            noDecay);
        area.AddRoom(6, 6, room);

        desc = @"The hallway opens up with an enchanting ceiling covered in luminescent crystals that emit a soft, ethereal glow. 
                 Along the sides, intricately carved statues stand sentinel, their eyes seeming to follow your every move. The 
                 walls bear ancient murals, vividly depicting long-lost legends. A small, crystal-clear fountain glows softly 
                 in the center of the room.";
        room = new Room(area,
            "The Castle Heart",
            FormatMultilineString(desc),
            noDecay);
        area.AddRoom(6, 7, room);

        desc = @"As you proceed down the hallway, the enchanting luminescence of the crystal-covered ceiling begins to fade, giving 
                 way to flickering torches that cast dancing shadows on the stone walls. The murals on the walls change, illustrating 
                 scenes of darker times and ominous tales of the castle's history. The air grows heavier, carrying a sense of 
                 foreboding as you venture deeper into the unknown.";
        room = new Room(area,
            "Hallway to Knight's Quarters",
            FormatMultilineString(desc),
            noDecay);
        area.AddRoom(5, 7, room);

        desc = @"As you descend the dimly lit staircase, the hallway leading to the living quarters greets you with a warm ambiance. 
                 Soft candlelight flickers gently, reflecting against suits of armor lining the walls. The hallway is decorated with 
                 ornate weapon displays, although the weapons that they are meant to hold have vanished through the years.";
        room = new Room(area,
            "Knight's Quarters",
            FormatMultilineString(desc),
            noDecay);
        area.AddRoom(4, 7, room);

        desc = @"The small room exudes a sense of chivalry and valor. A cozy hearth crackles softly, providing warmth and comfort 
                 during cold nights. The stone walls are adorned with intricate weaponry displays, showcasing the worn armor 
                 and battle-tested swords. An antique writing desk sits near a tall window, allowing ample natural light to 
                 illuminate parchments and maps strewn across its surface.";
        room = new Room(area,
            "Sir Aldric's Quarters",
            FormatMultilineString(desc),
            noDecay);
        area.AddRoom(3, 7, room);

        desc = @"The room exudes a rugged charm, with stone walls adorned with weaponry displays and tapestries depicting heroic battles. 
                 A sturdy wooden bed with a simple canopy stands against one wall, offering a place for the knight to rest after long 
                 days of adventure. An armor stand proudly showcases battle-worn armor, and a small weapon rack holds a variety of blades.";
        room = new Room(area,
            "Sir Gareth's Quarters",
            FormatMultilineString(desc),
            noDecay);
        area.AddRoom(4, 8, room);



        desc = @"As you proceed down the hallway, the enchanting luminescence of the crystal-covered ceiling begins to fade, giving 
                 way to flickering torches that cast dancing shadows on the stone walls. The murals on the walls change, illustrating 
                 scenes of fields of flowers and beautiful landscapes. Ornate sculptures line the walls of hallway, and the floor is 
                 covered in a velvet carpet.";
        room = new Room(area,
            "Hallway to Nobles Quarters",
            FormatMultilineString(desc),
            noDecay);
        area.AddRoom(7, 7, room);

        desc = @"As you descend the dimly lit staircase, the hallway leading to the living quarters greets you with a warm ambiance. 
                 Soft candlelight flickers gently, casting a comforting glow upon the well-maintained wooden floorboards. Rich 
                 tapestries adorned with intricate patterns line the walls, telling tales of the castle's opulent past. The air 
                 is infused with the scent of polished wood and freshly cut flowers, creating an inviting atmosphere that contrasts 
                 the castle's more foreboding sections.";
        room = new Room(area,
            "The Nobles Quarters",
            FormatMultilineString(desc),
            noDecay);
        area.AddRoom(8, 7, room);

        desc = @"This room offers a serene and ethereal atmosphere with pastel-colored tapestries, creating a dreamlike ambiance 
                 that soothes the weary soul. The large, canopied bed is draped in flowing silks, inviting restful slumber beneath 
                 its elegant canopy. Antique books and delicate trinkets grace the shelves, hinting at the occupant's love for art 
                 and literature. A gentle breeze carries the scent of lavender and roses from a nearby garden, permeating the room 
                 with nature's embrace.";
        room = new Room(area,
            "Lady Isabella's Quarters",
            FormatMultilineString(desc),
            noDecay);
        area.AddRoom(9, 7, room);

        desc = @"The room for the noble is a lavish display of opulence, with velvet drapes cascading from tall windows and gilded 
                 frames showcasing ancestral portraits. A regal four-poster bed, adorned with fine silk, serves as the centerpiece, 
                 while elegant tapestries and antique furniture exude timeless splendor. A crackling fireplace and plush armchairs 
                 provide an inviting space for intimate gatherings and noble affairs.";
        room = new Room(area,
            "Lord Rockwell's Quarters",
            FormatMultilineString(desc),
            noDecay);
        area.AddRoom(8, 8, room);




        desc = @"The hallway ahead leads to the Great Hall, the center of the castle. In ancient times, the king would rule over
                 his subjects from his throne in the Great Hall. As you approach the great wooden doors, you can feel the spirits 
                 of those who once lived in this castle. You should prepare yourself for what is to come.";
        room = new Room(area,
            "Entrance to The Great Hall",
            FormatMultilineString(desc),
            noDecay);
        area.AddRoom(6, 8, room);

        desc = @"You arrive into the Great Hall. The walls are decorated lavishly with embellished paintings of ancient battles, 
                 and a velvet carpet leads you towards the end of the hall. On the opposite side of the room you see a throne, 
                 glistening in the light, in stark contrast to the lack of light in the rest of the room, giving a forboding feeling.";
        room = new Room(area,
            "The Great Hall (South)",
            FormatMultilineString(desc),
            noDecay);
        area.AddRoom(6, 9, room);

        desc = @"The carpet leads to a set of stairs leading up to the singlular throne in the room. In contrast to the earlier half 
                 of the room, this section is nearly completely bare. Torch sconces light the way, although they glow with a faint blue light.
                 You feel a strange sense of calm and quiet acceptence of the coming battle. There's no turning back now. ";
        room = new Room(area,
            "The Great Hall",
            FormatMultilineString(desc),
            noDecay);
        area.AddRoom(6, 10, room);

        AddRooms(area);
    }

    public static void AddRooms(Area area)
    {
        SQLiteConnection data = new SQLiteConnection("Data Source=data.db");
        data.Open();
        var command = data.CreateCommand();
        command.CommandText =
        @"
            INSERT INTO areas (areaName, width, height, startingRoomID)
                VALUES ($areaName, $width, $height, -1)
        ";
        command.Parameters.AddWithValue("$areaName", area.Name);
        command.Parameters.AddWithValue("$width", area.GetWidth());
        command.Parameters.AddWithValue("$height", area.GetHeight());
        command.ExecuteNonQuery();
        data.Close();


        int areaID = Database.instance.GetAreaID(area.Name);

        data.Open();
        for (int x = 0; x < area.GetWidth(); x++)
        {
            for (int y = 0; y < area.GetHeight(); y++)
            {
                Room room = area.GetRoom(x, y);
                if (room is not null)
                {
                    command.CommandText =
                    @"
                        INSERT INTO rooms (roomName, roomDesc, areaID, roomIDUp, roomIDDown, roomIDLeft, roomIDRight, x, y)
                            VALUES ($roomName, $roomDesc, $areaID, -1, -1, -1, -1, $x, $y)
                    ";
                    command.Parameters.AddWithValue("$areaName", area.Name);
                    command.Parameters.AddWithValue("$roomName", room.Name);
                    command.Parameters.AddWithValue("$roomDesc", room.Description);
                    command.Parameters.AddWithValue("$areaID", areaID);
                    command.Parameters.AddWithValue("$x", x);
                    command.Parameters.AddWithValue("$y", y);
                    command.ExecuteNonQuery();
                }
            }
        }
        data.Close();

        for (int x = 0; x < area.GetWidth(); x++)
        {
            for (int y = 0; y < area.GetHeight(); y++)
            {
                Room room = area.GetRoom(x, y);
                if (room is not null)
                {
                    int left = Database.instance.GetRoomID(x - 1, y, Database.instance.GetAreaID(area.Name));
                    int right = Database.instance.GetRoomID(x + 1, y, Database.instance.GetAreaID(area.Name));
                    int up = Database.instance.GetRoomID(x, y + 1, Database.instance.GetAreaID(area.Name));
                    int down = Database.instance.GetRoomID(x, y - 1, Database.instance.GetAreaID(area.Name));

                    Console.Write(x);
                    Console.Write(" ");
                    Console.Write(y);
                    Console.Write(":  ");
                    Console.Write(left);
                    Console.Write(" ");
                    Console.Write(right);
                    Console.Write(" ");
                    Console.Write(up);
                    Console.Write(" ");
                    Console.Write(down);
                    Console.Write(" (");

                    data.Open();
                    command.CommandText =
                    @"
                        UPDATE rooms
                        SET roomIDLeft = $left,
                            roomIDRight = $right,
                            roomIDUp = $up,
                            roomIDDown = $down
                        WHERE x = $x AND y = $y
                    ";
                    command.Parameters.AddWithValue("$left", left);
                    command.Parameters.AddWithValue("$right", right);
                    command.Parameters.AddWithValue("$up", up);
                    command.Parameters.AddWithValue("$down", down);
                    command.Parameters.AddWithValue("$x", x);
                    command.Parameters.AddWithValue("$y", y);
                    Console.Write(command.ExecuteNonQuery());
                    Console.Write(")\n");
                    data.Close();
                }
            }
        }
        int startingRoomID = Database.instance.GetRoomID(6, 1, areaID);

        data.Open();
        command.CommandText =
        @"
            UPDATE areas
            SET startingRoomID = $startingRoomID
            WHERE areaName = $areaName
        ";
        command.Parameters.AddWithValue("$startingRoomID", startingRoomID);
        command.Parameters.AddWithValue("$areaName", area.Name);
        command.ExecuteNonQuery();
        data.Close();
    }



    static string FormatMultilineString(string str)
    {
        string[] lines = str.Split(
            new string[] { "\r\n", "\r", "\n" },
            StringSplitOptions.None
        );

        for (int i = 0; i < lines.Length; i++) {
            lines[i] = lines[i].Trim();
        }

        return string.Join(" ", lines);
    }
}