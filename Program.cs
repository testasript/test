
using System;
using System.Collections.Generic;
using LeagueSharp;
using LeagueSharp.Common;

namespace Kayle
{
    internal class Program
    {
        public static string ChampionName = "Kayle";
        public static Orbwalking.Orbwalker Orbwalker;
        public static List<Spell> SpellList = new List<Spell>();
        public static Spell Q;
        public static Spell W;
        public static Spell E;
        public static Spell R;
        public static Menu Config;

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += OnGameLoad;
        }

        private static void OnGameLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName != ChampionName)
            {
                return;
            }

            Q = new Spell(SpellSlot.Q, 650f);
            W = new Spell(SpellSlot.W, 900f);
            E = new Spell(SpellSlot.E, 525f);
            R = new Spell(SpellSlot.R, 900f);

            SpellList.Add(Q);
            SpellList.Add(W);
            SpellList.Add(E);
            SpellList.Add(R);

            Config = new Menu(ChampionName, ChampionName, true);
              var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
                TargetSelector.AddToMenu(targetSelectorMenu);
                Config.AddSubMenu(targetSelectorMenu);
                Config.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));
                  Orbwalker = new Orbwalking.Orbwalker(Config.SubMenu("Orbwalking"));
              Config.AddSubMenu(new Menu("Combo", "Combo"));
                Config.SubMenu("Combo").AddItem(new MenuItem("UseQCombo", "Use Q").SetValue(true));
                Config.SubMenu("Combo").AddItem(new MenuItem("UseWCombo", "Use W").SetValue(true));
                Config.SubMenu("Combo").AddItem(new MenuItem("UseECombo", "Use E").SetValue(true));
                Config.SubMenu("Combo").AddItem(new MenuItem("UseRCombo", "Use R").SetValue(true));
                Config.SubMenu("Combo")
                 .AddItem(new MenuItem("ComboActive", "Combo!").SetValue(new KeyBind(32, KeyBindType.Press)));
              Config.AddToMainMenu();

              Game.OnUpdate += OnGameUpdate;
        }

        private static void OnGameUpdate(EventArgs args)
        {
            if (Config.Item("ComboActive").GetValue<KeyBind>().Active)
            {
                var target = TargetSelector.GetTarget(800f, TargetSelector.DamageType.Magical);
                if (target != null)
                {
                    var comboActive = Config.Item("ComboActive").GetValue<KeyBind>().Active;
                    if (((comboActive && Config.Item("UseQCombo").GetValue<bool>())) && Q.IsReady())
                        if (ObjectManager.Player.Mana < 135)
                            if (target.Health < 600)
                            {
                                Q.CastOnUnit(target);
                            }
                            else
                            {
                                
                            }
                        else
                        {
                            Q.CastOnUnit(target);
                        }

                    if (((comboActive && Config.Item("UseWCombo").GetValue<bool>())) &&  W.IsReady() && target.Distance(ObjectManager.Player.Position) > 525)
                        if (target.Health < 600 && ObjectManager.Player.Mana > 235)
                        {
                            W.Cast(ObjectManager.Player);
                        }
                        else if (target.Health < 300 && ObjectManager.Player.Mana > 145)
                        {
                            W.Cast(ObjectManager.Player);
                        }

                    if (((comboActive && Config.Item("UseECombo").GetValue<bool>())) && E.IsReady() && target.Distance(ObjectManager.Player.Position) < 650)
                    {
                        E.Cast();
                    }

                    if (((comboActive && Config.Item("UseRCombo").GetValue<bool>())) && R.IsReady())
                        if (ObjectManager.Player.Health < 200 && target.Health > 200)
                        {
                            R.Cast(ObjectManager.Player);
                        }
                }
            }
        }
    }
}
