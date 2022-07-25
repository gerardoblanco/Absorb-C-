//PUT CHECKFOR FOUR-OF-A-KIND JUST AFTER play_card() IN PICTUREBOX CLICK EVENT HANDLER

// fix delays and orientation of cards for ai. also ai sometimes doesnt show delay when playing card
// test win conditions

//show card before playing it (ai) from the c.i.h so that the user has a better feel for which cards are being played on the pile
using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Absorb
{
    public partial class frmAbsorb : Form
    {
        enum Rank { one, two, three, four, five, six, seven, eight, nine, ten, eleven, twelve, thirteen };

        class Card
        {
            //public Rank rank;
            public int intrank;
            public int num_suits = 4;
            public int num_ranks = 13;
        };

        class Deck
        {
            //pickup pile
            public List<Card> cards = new List<Card>();

            public List<Card> playpile = new List<Card>();
            public int max_deck_size = 52;
        };

        class Player
        {
            public List<Card> facedown_hand = new List<Card>();
            public List<Card> faceup_hand = new List<Card>();
            public List<Card> cards_in_hand = new List<Card>();
            public List<Card> temporganised = new List<Card>();
        };

        class Game
        {
            public List<Player> players = new List<Player>();
            public Deck deck = new Deck();
            public int num_players = 4;
            public int starting_num_cards_per_hand = 3;
        };

        Game game = new Game();
        int playernumber = 0;
        //always set to 0 or a multiple of 3 so that it points to pictureBox1 after finished using it
        int cardinhandindex = 0;

        //variable to indicate if pile should be burned
        bool played9 = false;
        bool game_over = false;
        bool justplayed = false;

        System.Media.SoundPlayer playMusic = new System.Media.SoundPlayer(Properties.Resources.halo);

        bool playSound = false;

        frmRules showRulesForm = new frmRules();

        public frmAbsorb()
        {
            InitializeComponent();
            play();
        }

        void play()
        {
            initialise_game(game);
            deal_cards(game);
            print_game(game);

            startGameMusic();

            this.Icon = Properties.Resources.cardback1;
        }

        void startGameMusic()
        {
            playMusic.PlayLooping();
            playSound = true;

        }
        void initialise_deck(Deck deck)
        {
            deck.cards.Clear();
            deck.playpile.Clear();
            game.players.Clear();
            Card card = new Card();

            for (int amount = 0; amount < card.num_suits; amount++)
            {
                for (int rank = 0; rank < card.num_ranks; rank++)
                {
                    //assigning the rank attribute of card to the type conversion between the number and 
                    //the enumerated type associated with that number

                    //card.rank = (Rank)(rank + 1);
                    //card.intrank = rank+1;
                    deck.cards.Add(new Card() { intrank = rank + 1 });
                }
            }
            
        }

        void print_card(Card card)
        {
            Console.WriteLine("Rank: " + card.intrank);
        }

        void print_deck(Deck deck)
        {
            //for each card in deck print rank of card. Deck is a vector of cards 
            //and additional pieces of information-
            //card is in data member of deck called cards.

            Console.WriteLine("pickup pile: ");
            for(int i = 0; i < deck.cards.Count; i++)
            {
                print_card(deck.cards[i]);
            }

            Console.WriteLine("playing pile: ");
            for (int i = 0; i < deck.playpile.Count; i++)
            {
                print_card(deck.playpile[i]);
            }

        }

        void shuffle(Deck deck)
        {
            Deck shuffled = new Deck();
            Random rnd = new Random();
            int maxnum = 52;

            while (deck.cards.Count != 0)
            {
                int rand_index = rnd.Next(0, maxnum);
                shuffled.cards.Add(new Card() { intrank = deck.cards[rand_index].intrank });
                //remove selected card using erase method, calling begin function first 
                //and then adding the desired index to it
                deck.cards.RemoveAt(rand_index);
                maxnum--;
            }
            deck.cards.AddRange(shuffled.cards);
        }

        void setBacks(string picboxname)
        {
            foreach (var pictureBox in Controls.OfType<PictureBox>())
            {
                if ((pictureBox.Name == "pictureBox7") || (pictureBox.Name == "pictureBox8") || (pictureBox.Name == "pictureBox9") ||
                    (pictureBox.Name == "pictureBox10") || (pictureBox.Name == "pictureBox11") || (pictureBox.Name == "pictureBox12") ||
                        (pictureBox.Name == "pictureBox16") || (pictureBox.Name == "pictureBox17") || (pictureBox.Name == "pictureBox18") ||
                        (pictureBox.Name == "pictureBox19") || (pictureBox.Name == "pictureBox20") || (pictureBox.Name == "pictureBox21") ||
                        (pictureBox.Name == "pictureBox25") || (pictureBox.Name == "pictureBox26") || (pictureBox.Name == "pictureBox27") ||
                        (pictureBox.Name == "pictureBox28") || (pictureBox.Name == "pictureBox29") || (pictureBox.Name == "pictureBox30") ||
                        (pictureBox.Name == "pictureBox34") || (pictureBox.Name == "pictureBox35") || (pictureBox.Name == "pictureBox36"))
                {
                    pictureBox.Image = Properties.Resources.cardback;

                    int picboxnum = get_picbox_num(pictureBox);
                    if ((picboxnum > 9 && picboxnum < 19) || (picboxnum > 27 && picboxnum < 37))
                    {
                        Image img = pictureBox.Image;
                        img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        pictureBox.Image = img;
                    }
                }
            }
        }

        int get_picbox_num(PictureBox p)
        {
            char[] charsToTrim = { 'p', 'i', 'c', 't', 'u', 'r', 'e', 'B', 'o', 'x' };
            string cleanString = p.Name.TrimStart(charsToTrim);
            int picboxnum = 0;

            if (Int32.TryParse(cleanString, out picboxnum))
            {
                return picboxnum;
            }

            return picboxnum;
        }

        int get_picbox_card_rank(PictureBox p)
        {
            string chosen_card_rank = (string)p.Image.Tag;
            int chosen_card_tempnum = 0;

            if (Int32.TryParse(chosen_card_rank, out chosen_card_tempnum))
            {
                return chosen_card_tempnum;
            }

            return chosen_card_tempnum;
        }

        void rotateImage(PictureBox p)
        {
            int picboxnum = get_picbox_num(p);
            if ((picboxnum > 9 && picboxnum < 19) || (picboxnum > 27 && picboxnum < 37))
            {
                Image img = p.Image;
                img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                p.Image = img;
            }
        }

        void setImage(int rank, string picboxname)
        {
            foreach (var pictureBox in Controls.OfType<PictureBox>())
            {
                if (pictureBox.Name == picboxname)
                {
                    switch (rank)
                    {
                        case -3:
                            pictureBox.Image = Properties.Resources.cardback;
                            pictureBox.Image.Tag = "-3";
                            rotateImage(pictureBox);
                            break;

                        case -2:
                            pictureBox.Image = Properties.Resources.emptypickup;
                            pictureBox.Image.Tag = "-2";
                            rotateImage(pictureBox);
                            break;

                        case 1:
                            pictureBox.Image = Properties.Resources.one;
                            pictureBox.Image.Tag = "1";
                            rotateImage(pictureBox);
                            break;
                        case 2:
                            pictureBox.Image = Properties.Resources.two;
                            pictureBox.Image.Tag = "2";
                            rotateImage(pictureBox);
                            break;
                        case 3:
                            pictureBox.Image = Properties.Resources.three;
                            pictureBox.Image.Tag = "3";
                            rotateImage(pictureBox);
                            break;
                        case 4:
                            pictureBox.Image = Properties.Resources.four;
                            pictureBox.Image.Tag = "4";
                            rotateImage(pictureBox);
                            break;
                        case 5:
                            pictureBox.Image = Properties.Resources.five;
                            pictureBox.Image.Tag = "5";
                            rotateImage(pictureBox);
                            break;
                        case 6:
                            pictureBox.Image = Properties.Resources.six;
                            pictureBox.Image.Tag = "6";
                            rotateImage(pictureBox);
                            break;
                        case 7:
                            pictureBox.Image = Properties.Resources.seven;
                            pictureBox.Image.Tag = "7";
                            rotateImage(pictureBox);
                            break;
                        case 8:
                            pictureBox.Image = Properties.Resources.eight;
                            pictureBox.Image.Tag = "8";
                            rotateImage(pictureBox);
                            break;
                        case 9:
                            pictureBox.Image = Properties.Resources.nine;
                            pictureBox.Image.Tag = "9";
                            rotateImage(pictureBox);
                            break;
                        case 10:
                            pictureBox.Image = Properties.Resources.ten;
                            pictureBox.Image.Tag = "10";
                            rotateImage(pictureBox);
                            break;
                        case 11:
                            pictureBox.Image = Properties.Resources.eleven;
                            pictureBox.Image.Tag = "11";
                            rotateImage(pictureBox);
                            break;
                        case 12:
                            pictureBox.Image = Properties.Resources.twelve;
                            pictureBox.Image.Tag = "12";
                            rotateImage(pictureBox);
                            break;
                        case 13:
                            pictureBox.Image = Properties.Resources.thirteen;
                            pictureBox.Image.Tag = "13";
                            rotateImage(pictureBox);
                            break;
                        default:
                            break;
                    }
                    
                }
            }
        }

        bool deal_cards(Game game)
        {
            PickupPile.Image = Properties.Resources.pickup;
            Image img = PickupPile.Image;
            img.RotateFlip(RotateFlipType.Rotate90FlipNone);
            PickupPile.Image = img;

            string picboxname = "pictureBox";
            string suffix;
            int picboxcounter = 1;
            
            if (game.deck.cards.Count < game.num_players * game.starting_num_cards_per_hand)
            {
                return false;
            }

            for (int player = 0; player < game.num_players; player++)
            {
                for (int card = 0; card < game.starting_num_cards_per_hand; card++)
                {
                    suffix = picboxcounter.ToString();
                    picboxname = "pictureBox" + suffix;
                    setImage(game.deck.cards[0].intrank, picboxname);
                    setBacks(picboxname);

                    game.players[player].cards_in_hand.Add(new Card() { intrank = game.deck.cards[0].intrank });
                    game.deck.cards.RemoveAt(0);

                    picboxcounter++;
                }

                for (int card = 0; card < game.starting_num_cards_per_hand; card++)
                {
                    suffix = picboxcounter.ToString();
                    picboxname = "pictureBox" + suffix;
                    setImage(game.deck.cards[0].intrank, picboxname);
                    setBacks(picboxname);

                    game.players[player].faceup_hand.Add(new Card() { intrank = game.deck.cards[0].intrank });
                    game.deck.cards.RemoveAt(0);

                    picboxcounter++;
                }

                for (int card = 0; card < game.starting_num_cards_per_hand; card++)
                {
                    suffix = picboxcounter.ToString();
                    picboxname = "pictureBox" + suffix;
                    setImage(game.deck.cards[0].intrank, picboxname);
                    setBacks(picboxname);

                    game.players[player].facedown_hand.Add(new Card() { intrank = game.deck.cards[0].intrank });
                    game.deck.cards.RemoveAt(0);

                    picboxcounter++;
                }
            }

            //place first card on the playing pile
            suffix = picboxcounter.ToString();
            picboxname = "pictureBox" + suffix;
            setImage(game.deck.cards[0].intrank, picboxname);
            game.deck.playpile.Add(new Card() { intrank = game.deck.cards[0].intrank });
            game.deck.cards.RemoveAt(0);

            return true;
        }

        void print_hand(List<Card> hand) {

            for (int i = 0; i < hand.Count; i++)
            {
                print_card(hand[i]);
            }
        }

        void initialise_players(Game game)
        {

            for (int player = 0; player < game.num_players; player++)
            {
                Player new_player = new Player();
                game.players.Add(new Player());
            }
        }

        void initialise_game(Game game)
        {
            initialise_deck(game.deck);
            shuffle(game.deck);
            initialise_players(game);
        }

        void print_game(Game game) {

            for (int player = 0; player<game.num_players; player++)
            {
                Console.WriteLine("In hand: ");
                print_hand(game.players[player].cards_in_hand);
                Console.WriteLine("Faceup: ");
                print_hand(game.players[player].faceup_hand);
                Console.WriteLine("Facedown: ");
                print_hand(game.players[player].facedown_hand);
                

                Console.WriteLine("");

            }
            print_deck(game.deck);
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("");
            Console.WriteLine("-----------------------------------------------");
        }

        bool check_to_take(Game game)
        {
            if (check_for_positives_in_hand(game) != 0)
            {
                for (int i = 0; i < game.players[playernumber].cards_in_hand.Count; i++)
                {
                    if (is_valid(game.players[playernumber].cards_in_hand[i].intrank))
                    {
                        return true;
                    }
                }
            }
            else if (check_for_positives_up(game) != 0)
            {
                for (int i = 0; i < game.players[playernumber].faceup_hand.Count; i++)
                {
                    if (is_valid(game.players[playernumber].faceup_hand[i].intrank))
                    {
                        return true;
                    }
                }
            }
            else if (check_for_positives_down(game) != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            //figure out a way to validate facedown cards


            return false;
        }

        void take_playpile()
        {
            if (playernumber == 0)
            {
                MessageBox.Show("You have no valid cards... ABSORB!");
            }

            //iterate through the cards in hand, if there are any -2's, substitute with the top card
            //of playpile

            for (int i = 0; i < game.players[playernumber].cards_in_hand.Count; i++)
            {
                if (game.players[playernumber].cards_in_hand[i].intrank == -2 && game.deck.playpile.Count != 0)
                {
                    game.players[playernumber].cards_in_hand[i].intrank = game.deck.playpile[0].intrank;

                    if(playernumber == 0)
                    {
                        if (i == cardinhandindex)
                        {
                            setImage(game.deck.playpile[0].intrank, "pictureBox1");
                        }
                        else if (i == (cardinhandindex + 1))
                        {
                            setImage(game.deck.playpile[0].intrank, "pictureBox2");
                        }
                        else if (i == (cardinhandindex + 2))
                        {
                            setImage(game.deck.playpile[0].intrank, "pictureBox3");
                        }
                    }
                    
                    game.deck.playpile.RemoveAt(0);
                }
            }

            //if there are still any cards left in playpile, add them to the end of the cards in hand

            while (game.deck.playpile.Count != 0)
            {
                game.players[playernumber].cards_in_hand.Add(new Card() { intrank = game.deck.playpile[0].intrank });
                game.deck.playpile.RemoveAt(0);
            }

            setImage(-2, "pictureBox37");

        }

        bool is_valid(int rank)
        {
            if (rank != -2)
            {
                int pickup_tempnum = get_picbox_card_rank(pictureBox37);

                //play a 10 (burn)
                if (rank == 9 && pickup_tempnum != 2 && pickup_tempnum != 8)
                {
                    return true;
                }
                //3 forces a 3 or a 9
                else if (pickup_tempnum == 2 && (rank == 2 || rank == 8))
                {
                    return true;
                }
                //2 & 8 on anything but a 3
                else if ((rank == 1 || rank == 7) && pickup_tempnum != 2)
                {
                    return true;
                }
                //9 forces less than a 9
                else if (pickup_tempnum == 8 && rank < pickup_tempnum)
                {
                    return true;
                }
                else if (rank >= pickup_tempnum && pickup_tempnum != 2
                    && pickup_tempnum != 8)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        void play_card(Game game, PictureBox p)
        {
            //play card from hand onto pile
            int chosen_card_tempnum = get_picbox_card_rank(p);

            int temppickuptop = get_picbox_card_rank(pictureBox37);

            if(chosen_card_tempnum == 9)
            {
                played9 = true;
            }

            game.deck.playpile.Insert(0, new Card() { intrank = chosen_card_tempnum });
            setImage(chosen_card_tempnum, "pictureBox37");

            //begin 'removal' of card from hand
            int picboxnum = get_picbox_num(p);

            if (picboxnum <= 3)
            {
                int cardindex = picboxnum - 1;
                if (cardindex != -1)
                {
                    //key. -2 = empty card slot. This is to avoid having to delete element from list 
                    //which would cause the list to shift
                    game.players[playernumber].cards_in_hand[cardinhandindex + cardindex].intrank = -2;
                    string picboxname = (string)p.Name;
                    setImage(-2, picboxname);
                    wait(1000);
                    draw_card(game, picboxname);
                }
            }
            else if (picboxnum <= 6)
            {
                int cardindex = picboxnum - 4;
                if (cardindex != -1)
                {
                    game.players[playernumber].faceup_hand[cardindex].intrank = -2;
                    string picboxname = (string)p.Name;
                    setImage(-2, picboxname);
                }
            }
            else if (picboxnum <= 9)
            {
                int cardindex = picboxnum - 7;
                if (cardindex != -1)
                {
                    game.players[playernumber].facedown_hand[cardindex].intrank = -2;
                    string picboxname = (string)p.Name;
                    setImage(-2, picboxname);
                }

            }

            if (chosen_card_tempnum == 7)
            {
                wait(500);
                setImage(temppickuptop, "pictureBox37");
            }

            if (game.deck.playpile.Count > 3)
            {
                if (game.deck.playpile[0].intrank == game.deck.playpile[1].intrank
                && game.deck.playpile[1].intrank == game.deck.playpile[2].intrank
                && game.deck.playpile[2].intrank == game.deck.playpile[3].intrank)
                {
                    played9 = true;
                }
            }

            if (check_for_positives_in_hand(game) == 0 &&
               check_for_positives_down(game) == 0 &&
                check_for_positives_up(game) == 0)
            {
                game_over = true;
            }
            
        }

        int check_for_positives_in_hand(Game game)
        {
            int positives = 0;
            for (int i = 0; i < game.players[playernumber].cards_in_hand.Count; i++)
            {
                if (game.players[playernumber].cards_in_hand[i].intrank > 0)
                {
                    positives++;
                }
            }
            return positives;
        }

        int check_for_positives_up(Game game)
        {
            int positives = 0;
            for (int i = 0; i < game.players[playernumber].faceup_hand.Count; i++)
            {
                if (game.players[playernumber].faceup_hand[i].intrank > 0)
                {
                    positives++;
                }
            }
            return positives;
        }

        int check_for_positives_down(Game game)
        {
            int positives = 0;
            for (int i = 0; i < game.players[playernumber].facedown_hand.Count; i++)
            {
                if (game.players[playernumber].facedown_hand[i].intrank > 0)
                {
                    positives++;
                }
            }
            return positives;
        }

        void draw_card(Game game, string picboxname)
        {
            if (check_for_positives_in_hand(game) < 3 && game.deck.cards.Count != 0)
            {
                //for each card in the cards in hand, if there is a '-2', draw a card from pickup pile
                // and replace the '-2' with that card. Delete top card of the pickup pile
                for (int i = 0; i < game.players[playernumber].cards_in_hand.Count; i++)
                {
                    if (game.players[playernumber].cards_in_hand[i].intrank == -2 && check_for_positives_in_hand(game) < 3)
                    {
                        int picboxnum = ((playernumber + 1)* 9) - 8 + i;
                        string concatenated = "pictureBox" + picboxnum.ToString();

                        //string tempcardindex = (i+1).ToString();
                        //string concatenated = "pictureBox" + tempcardindex;

                        game.players[playernumber].cards_in_hand[i].intrank = game.deck.cards[0].intrank;
                        setImage(game.deck.cards[0].intrank, concatenated);
                        game.deck.cards.RemoveAt(0);
                    }
                }
            }
            if (game.deck.cards.Count == 0)
            {
                setImage(-2, (string)PickupPile.Name);
            }
        }

        void delete_playpile()
        {
            while (game.deck.playpile.Count != 0)
            {
                game.deck.playpile.RemoveAt(0);
            }

            setImage(-2, "pictureBox37");
        }

        void wait(int milliseconds)
        {
            var timer1 = new System.Windows.Forms.Timer();
            if (milliseconds == 0 || milliseconds < 0) return;

            // Console.WriteLine("start wait timer");
            timer1.Interval = milliseconds;
            timer1.Enabled = true;
            timer1.Start();

            timer1.Tick += (s, e) =>
            {
                timer1.Enabled = false;
                timer1.Stop();
                // Console.WriteLine("stop wait timer");
            };

            while (timer1.Enabled)
            {
                Application.DoEvents();
            }
        }

        void refresh_ai()
        {
            int picboxnum = 0;
            string concatenated;
            if (check_for_positives_in_hand(game) >= 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    picboxnum = ((playernumber + 1) * 9) - 8 + i;
                    concatenated = "pictureBox" + picboxnum.ToString();
                    setImage(-3, concatenated);
                }
            }
            else if (check_for_positives_in_hand(game) == 2)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (i < 2)
                    {
                        picboxnum = ((playernumber + 1) * 9) - 8 + i;
                        concatenated = "pictureBox" + picboxnum.ToString();
                        setImage(-3, concatenated);
                    }
                    else
                    {
                        picboxnum = ((playernumber + 1) * 9) - 8 + i;
                        concatenated = "pictureBox" + picboxnum.ToString();
                        setImage(-2, concatenated);
                    }
                    
                }
            }
            else if (check_for_positives_in_hand(game) == 1)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (i < 1)
                    {
                        picboxnum = ((playernumber + 1) * 9) - 8 + i;
                        concatenated = "pictureBox" + picboxnum.ToString();
                        setImage(-3, concatenated);
                    }
                    else
                    {
                        picboxnum = ((playernumber + 1) * 9) - 8 + i;
                        concatenated = "pictureBox" + picboxnum.ToString();
                        setImage(-2, concatenated);
                    }

                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    picboxnum = ((playernumber + 1) * 9) - 8 + i;
                    concatenated = "pictureBox" + picboxnum.ToString();
                    setImage(-2, concatenated);
                }
            }
        }

        void ai_manager(Game game)
        {
            playernumber = (playernumber + 1) % game.num_players;

            while (playernumber != 0 && !game_over)
            {
                wait(300);
                displayBox.Text = "Player " + (playernumber +  1) + "'s turn!";
                wait(1000);
                
                if (check_to_take(game))
                {
                    ai_play_card(game);

                }
                if(justplayed == false)
                {
                    //if there are still facedown cards left, pick up the first non empty one and place it in hand.
                    if (check_for_positives_up(game) == 0 && check_for_positives_in_hand(game) == 0
                        && check_for_positives_down(game) != 0)
                    {
                        bool card_found = false;
                        for (int i = 0; i < game.players[playernumber].facedown_hand.Count; i++)
                        {
                            if (game.players[playernumber].facedown_hand[i].intrank != -2 && card_found == false)
                            {
                                int picboxnum = ((playernumber + 1) * 9) - 2 + i;
                                string concatenated = "pictureBox" + picboxnum.ToString();

                                setImage(game.players[playernumber].facedown_hand[i].intrank, concatenated);
                                wait(2000);
                                game.players[playernumber].cards_in_hand.Add(new Card() { intrank = game.players[playernumber].facedown_hand[i].intrank });
                                game.players[playernumber].facedown_hand[i].intrank = -2;
                                setImage(-2, concatenated);
                                
                                card_found = true;
                                wait(1000);
                            }
                        }
                    }
                    take_playpile();
                    refresh_ai();
                }

                justplayed = false;

                if (played9)
                {
                    displayBox.Text = "Player " + (playernumber + 1) + " burns the pile!";
                    wait(400);
                    delete_playpile();
                    played9 = false;
                }
                else if(!game_over)
                {
                    playernumber = (playernumber + 1) % game.num_players;
                }
                
            }
            if (!game_over)
            {
                displayBox.Text = "Your turn!";
            }
            else
            {
                displayBox.Text = "Player " + playernumber + " wins!";
                restart();
            }
        }

        void ai_play_card(Game game)
        {
            bool card_found = false;
            int picboxnum = 0;
            int chosen_card_tempnum = 0;
            string concatenated;
            int temppickuptop = get_picbox_card_rank(pictureBox37);

            if (check_for_positives_in_hand(game) != 0)
            {
                for (int i = 0; i < game.players[playernumber].cards_in_hand.Count; i++)
                {
                    if (is_valid(game.players[playernumber].cards_in_hand[i].intrank) && card_found == false)
                    {
                        //to get picboxnumber of the card that is valid out of c.i.h of ai:
                        //((playernumber+1)*9)-8(+i) .... but this will only be useful for faceupcards,
                        //as the rest of the ai cards will be hidden from the player


                        //this wont work if you play a card that isnt in the first three in hand because the 
                        //picbox you'll be changing will be one that isnt from the c.i.h as i can be greater than 3
                        // if the c.i.h count is greater than 3
                        //FIX ITTTT
                        picboxnum = ((playernumber + 1) * 9) - 8 + i;
                        concatenated = "pictureBox" + picboxnum.ToString();
                        chosen_card_tempnum = game.players[playernumber].cards_in_hand[i].intrank;

                        if (game.players[playernumber].cards_in_hand[i].intrank == 9)
                        {
                            played9 = true;
                        }
                        if (i > 2)
                        {
                            picboxnum = ((playernumber + 1) * 9) - 8 + 2;
                            concatenated = "pictureBox" + picboxnum.ToString();
                        }

                        setImage(game.players[playernumber].cards_in_hand[i].intrank, concatenated);
                        wait(800);
                        game.deck.playpile.Insert(0, new Card() { intrank = game.players[playernumber].cards_in_hand[i].intrank });
                        setImage(game.players[playernumber].cards_in_hand[i].intrank, "pictureBox37");
                        game.players[playernumber].cards_in_hand[i].intrank = -2;
                        //remove later
                        if (i < 3)
                        {
                            setImage(-2, concatenated);
                        }

                        displayBox.Text = "Player " + (playernumber + 1) + " plays a " + chosen_card_tempnum + "!";

                        wait(1000);
                        draw_card(game, concatenated);
                        card_found = true;
                        justplayed = true;
                    }
                }
            }
            else if (check_for_positives_up(game) != 0)
            {
                for (int i = 0; i < game.players[playernumber].faceup_hand.Count; i++)
                {
                    if (is_valid(game.players[playernumber].faceup_hand[i].intrank) && card_found == false)
                    {
                        picboxnum = ((playernumber + 1) * 9) - 5 + i;
                        concatenated = "pictureBox" + picboxnum.ToString();
                        chosen_card_tempnum = game.players[playernumber].cards_in_hand[i].intrank;

                        if (game.players[playernumber].faceup_hand[i].intrank == 9)
                        {
                            played9 = true;
                        }

                        game.deck.playpile.Insert(0, new Card() { intrank = game.players[playernumber].faceup_hand[i].intrank });
                        setImage(game.players[playernumber].faceup_hand[i].intrank, "pictureBox37");
                        game.players[playernumber].faceup_hand[i].intrank = -2;
                        setImage(game.players[playernumber].faceup_hand[i].intrank, concatenated);

                        wait(1000);
                        draw_card(game, concatenated);
                        card_found = true;
                        justplayed = true;
                    }
                }
            }
            else if (check_for_positives_down(game) != 0)
            {
                for (int i = 0; i < game.players[playernumber].facedown_hand.Count; i++)
                {
                    if (is_valid(game.players[playernumber].facedown_hand[i].intrank) && card_found == false)
                    {
                        picboxnum = ((playernumber + 1) * 9) - 2 + i;
                        concatenated = "pictureBox" + picboxnum.ToString();
                        chosen_card_tempnum = game.players[playernumber].cards_in_hand[i].intrank;

                        if (game.players[playernumber].facedown_hand[i].intrank == 9)
                        {
                            played9 = true;
                        }

                        setImage(game.players[playernumber].facedown_hand[i].intrank, concatenated);
                        wait(2000);
                        game.deck.playpile.Insert(0, new Card() { intrank = game.players[playernumber].facedown_hand[i].intrank });
                        setImage(game.players[playernumber].facedown_hand[i].intrank, "pictureBox37");
                        game.players[playernumber].facedown_hand[i].intrank = -2;
                        setImage(game.players[playernumber].facedown_hand[i].intrank, concatenated);
                        draw_card(game, concatenated);
                        card_found = true;

                        justplayed = true;
                    }
                }
                if (check_for_positives_down(game) == 0)
                {
                    game_over = true;
                }
            }
            else
            {
                game_over = true;
            }

            if (chosen_card_tempnum == 7)
            {
                wait(500);
                setImage(temppickuptop, "pictureBox37");
            }

            if (game.deck.playpile.Count > 3)
            {
                if (game.deck.playpile[0].intrank == game.deck.playpile[1].intrank
                && game.deck.playpile[1].intrank == game.deck.playpile[2].intrank
                && game.deck.playpile[2].intrank == game.deck.playpile[3].intrank)
                {
                    played9 = true;
                }
            }
            

            refresh_ai();
        }

        void restructure()
        {
            //attempt at removing empty slots so that only real values are in the list, and then re-setimage for first 3 pictureboxes
            //depending on the first 3 values
            for (int i = 0; i < game.players[playernumber].cards_in_hand.Count; i++)
            {
                if (game.players[playernumber].cards_in_hand[i].intrank == -2 && game.players[playernumber].cards_in_hand.Count > 3)
                {
                    game.players[playernumber].cards_in_hand.RemoveAt(i);
                    i--;
                }
            }

            while (game.players[playernumber].cards_in_hand.Count < 3)
            {
                game.players[playernumber].cards_in_hand.Add(new Card() { intrank = -2 });
            }
            cardinhandindex = 0;
        }

        void refresh_images()
        {
            string picboxname = "pictureBox";
            string suffix;
            int picboxcounter = 1;

            for (int i = 0; i < 3; i++)
            {
                suffix = picboxcounter.ToString();
                picboxname = "pictureBox" + suffix;
                setImage(game.players[0].cards_in_hand[cardinhandindex + i].intrank, picboxname);

                picboxcounter++;
            }

        }

        void restart()
        {
            DialogResult dialogResult = MessageBox.Show("Would you like to play again?", "GAME OVER", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                this.Controls.Clear();
                this.InitializeComponent();
                play();
                
            }
            else
            {
                Application.Restart();
                Environment.Exit(0);
            }
        }

        void show_right()
        {
            if ((cardinhandindex + 3) == game.players[0].cards_in_hand.Count || check_for_positives_in_hand(game) == 0)
            {
                displayBox.Text = "No more cards left to show!";
                return;
            }
            int remainder = (game.players[0].cards_in_hand.Count - cardinhandindex) % 3;

            if (game.players[0].cards_in_hand.Count > 3)
            {
                //if the number of remaining cards to show is equal to the remainder
                if ((cardinhandindex + 3 + remainder) == game.players[0].cards_in_hand.Count)
                {
                    //this will set cardinhandindex to be at the 3rd to last card of the list
                    //hence now being able to display the last 3 cards
                    cardinhandindex = cardinhandindex + remainder;

                    refresh_images();

                }
                else
                {
                    cardinhandindex = cardinhandindex + 3;

                    refresh_images();
                }
            }

        }

        void show_left()
        {
            if (cardinhandindex == 0 || check_for_positives_in_hand(game) == 0)
            {
                displayBox.Text = "No more cards left to show!";
                return;
            }
            
            if (game.players[0].cards_in_hand.Count > 3)
            {    
                //move left 3 at a time
                if(cardinhandindex - 3 >= 0)
                {
                    cardinhandindex = cardinhandindex - 3;

                    refresh_images();
                }
                else
                {
                    cardinhandindex = 0;

                    refresh_images();
                }
            }



            }

        void generic_playthrough(PictureBox p)
        {
            if (!game_over && playernumber == 0)
            {
                if (check_to_take(game))
                {
                    int picboxnum = get_picbox_num(p);
                    int tempfacedownindex = picboxnum - 7;

                    //if it corresponds to one of the facedown cards
                    // && p.Image == Properties.Resources.cardback
                    if (picboxnum <= 9 && picboxnum > 6)
                    {
                        setImage(game.players[playernumber].facedown_hand[tempfacedownindex].intrank, (string)p.Name);
                        wait(2000);
                    }

                    if (is_valid(get_picbox_card_rank(p)))
                    {
                        play_card(game, p);
                        restructure();
                        refresh_images();
                        if (played9)
                        {
                            wait(400);
                            delete_playpile();
                            playernumber = 3;
                            played9 = false;
                        }
                        if (!game_over)
                        {
                            ai_manager(game);
                        }
                        else
                        {
                            displayBox.Text = "YOU WON!";
                            restart();
                        }
                    }
                    else
                    {
                        if (picboxnum <= 9 && picboxnum > 6 && get_picbox_card_rank(p) != -2)
                        {
                            game.players[playernumber].cards_in_hand.Add(new Card() { intrank = get_picbox_card_rank(p) });
                            game.players[playernumber].facedown_hand[tempfacedownindex].intrank = -2;
                            setImage(-2, (string)p.Name);

                            take_playpile();
                            ai_manager(game);
                            restructure();
                            refresh_images();
                        }
                        
                    }
                    print_game(game);
                }
                else
                {
                    if (check_for_positives_down(game) == 0 && check_for_positives_up(game) == 0
                        && check_for_positives_in_hand(game) == 0)
                    {
                        game_over = true;
                        displayBox.Text = "YOU WON!";
                        restart();
                    }
                    else
                    {
                        take_playpile();
                        restructure();
                        print_game(game);
                        refresh_images();
                        ai_manager(game);
                    }

                }
            }
            
        }


        //Form Event Handlers:

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if(check_for_positives_in_hand(game) != 0)
            {
                generic_playthrough(pictureBox1);
            }
            else
            {
                MessageBox.Show("You have no cards left in hand!");
            }
            
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (check_for_positives_in_hand(game) != 0)
            {
                generic_playthrough(pictureBox2);
            }
            else
            {
                MessageBox.Show("You have no cards left in hand!");
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (check_for_positives_in_hand(game) != 0)
            {
                generic_playthrough(pictureBox3);
            }
            else
            {
                MessageBox.Show("You have no cards left in hand!");
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (check_for_positives_in_hand(game) != 0)
            {
                MessageBox.Show("You still have cards in hand to play!");
            }
            else
            {
                generic_playthrough(pictureBox4);
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (check_for_positives_in_hand(game) != 0)
            {
                MessageBox.Show("You still have cards in hand to play!");
            }
            else
            {
                generic_playthrough(pictureBox5);
            }
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            if (check_for_positives_in_hand(game) != 0)
            {
                MessageBox.Show("You still have cards in hand to play!");
            }
            else
            {
                generic_playthrough(pictureBox6);
            }
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            if (check_for_positives_in_hand(game) != 0)
            {
                MessageBox.Show("You still have cards in hand to play!");
            }
            else if(check_for_positives_up(game) != 0)
            {
                MessageBox.Show("You still have face-up cards left to play!");
            }
            else
            {
                generic_playthrough(pictureBox7);
            }
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            if (check_for_positives_in_hand(game) != 0)
            {
                MessageBox.Show("You still have cards in hand to play!");
            }
            else if (check_for_positives_up(game) != 0)
            {
                MessageBox.Show("You still have face-up cards left to play!");
            }
            else
            {
                generic_playthrough(pictureBox8);
            }
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            if (check_for_positives_in_hand(game) != 0)
            {
                MessageBox.Show("You still have cards in hand to play!");
            }
            else if (check_for_positives_up(game) != 0)
            {
                MessageBox.Show("You still have face-up cards left to play!");
            }
            else
            {
                generic_playthrough(pictureBox9);
            }
        }

        private void BtnRight_Click(object sender, EventArgs e)
        {
            show_right();
        }

        private void BtnLeft_Click(object sender, EventArgs e)
        {
            show_left();
        }

        private void toggleSoundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (playSound)
            {
                playMusic.Stop();
                playSound = false;
            }
            else
            {
                playMusic.PlayLooping();
                playSound = true;
            }
        }

        private void frmAbsorb_FormClosed(object sender, FormClosedEventArgs e)
        {
            playMusic.Stop();
            Application.ExitThread();
            System.Environment.Exit(0);
        }

        private void rulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // This block of code ensures only one rules form is open at any time.
            try
            {
                if (showRulesForm.Visible) // if rules form is already open.
                {
                    showRulesForm.BringToFront(); // brings form to front.
                }
                else
                {
                    showRulesForm.Show(); // opens rules form again.
                }
            }
            // showRulesForm.Visible() throws an exception if either frmRules has 
            // not been instantiated or if the user has closed the form.
            catch (Exception)
            {
                showRulesForm = new frmRules(); // creates new instance of rules form.
                showRulesForm.Show(); // displays form.
            }
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            this.InitializeComponent();
            play();
        }
    }
}
