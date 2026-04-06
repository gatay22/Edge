using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using Microsoft.Xna.Framework;

namespace NightsEdgePlugin;

[ApiVersion(2, 1)]
public class NightsEdge : TerrariaPlugin
{
    public override string Name => "Night's Edge New Attack";
    public override string Author => "User";
    public override Version Version => new Version(1, 0);

    public NightsEdge(Main game) : base(game) { }

    public override void Initialize()
    {
        // Hook saat pemain mengirim paket data (termasuk ayunan senjata)
        ServerApi.Hooks.NetGetData.Register(this, OnGetData);
    }

    private void OnGetData(GetDataEventArgs args)
    {
        if (args.MsgID == PacketTypes.PlayerSlot) // Cek penggunaan slot item
        {
            TSPlayer player = TShock.Players[args.Index];
            if (player == null || !player.Active) return;

            // ID 273 = Night's Edge
            if (player.SelectedItem.type == 273)
            {
                // Koordinat pemain
                float posX = player.X + (player.TPlayer.width / 2);
                float posY = player.Y + (player.TPlayer.height / 2);

                // Buat Proyekstil Demon Scythe (ID 45) mengarah ke posisi kursor pemain
                // Damage: 60, Knockback: 2f
                Projectile.NewProjectile(null, posX, posY, 5f, 5f, 45, 60, 2f, player.Index);
            }
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            ServerApi.Hooks.NetGetData.Deregister(this, OnGetData);
        }
        base.Dispose(disposing);
    }
}

