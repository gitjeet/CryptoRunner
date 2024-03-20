module rangeonft::geotagrandnft{
    use std::signer::address_of;
    use std::vector;
    use std::bcs;
    use aptos_framework::randomness;
    use aptos_token::token;
    use std::signer;
    use std::string::{Self, String};
    use aptos_token::token::TokenDataId;
    struct RandomLocation has key {
        rolls: vector<u64>,
    }

    struct ModuleData has key {
        token_data_id: TokenDataId,
    }


    const ENOT_AUTHORIZED: u64 = 1;

    fun init_module(source_account: &signer) {
        let collection_name = string::utf8(b"GeoTagNFT");
        let description = string::utf8(b"Geo Tag NFT minted and placed at random location of the user");
        let collection_uri = string::utf8(b"geotagnft");
        let token_name = string::utf8(b"geotagnft");
        let token_uri = string::utf8(b"geotagnft");
      
        let maximum_supply = 0;
        
        let mutate_setting = vector<bool>[ false, false, false ];

        token::create_collection(source_account, collection_name, description, collection_uri, maximum_supply, mutate_setting);

        let token_data_id = token::create_tokendata(
            source_account,
            collection_name,
            token_name,
            string::utf8(b""),
            0,
            token_uri,
            signer::address_of(source_account),
            1,
            0,
           
            token::create_token_mutability_config(
                &vector<bool>[ false, false, false, false, true ]
            ),

            vector<String>[string::utf8(b"given_to")],
            vector<vector<u8>>[b""],
            vector<String>[ string::utf8(b"address") ],
        );


        move_to(source_account, ModuleData {
            token_data_id,
        });
    }
    #[randomness]
    entry fun roll(account: signer) acquires RandomLocation {
        let addr = address_of(&account);
        let roll_history = if (exists<RandomLocation>(addr)) {
            move_from<RandomLocation>(addr)
        } else {
            RandomLocation { rolls: vector[] }
        };
        let new_roll = randomness::u64_range(0, 6);
        vector::push_back(&mut roll_history.rolls, new_roll);
        move_to(&account, roll_history);
    }

    #[view]
    #[randomness]
    public fun get_geolocation(
        
    ): (u64)  {
        // if this address doesn't have an Aptogotchi, throw error
       let new_roll = randomness::u64_range(0, 6);
        new_roll
    }
    public entry fun mint_geo_token(module_owner: &signer, receiver: &signer) acquires ModuleData {
 
    let module_data = borrow_global_mut<ModuleData>(@mint_nft);
    let token_id = token::mint_token(module_owner, module_data.token_data_id, 1);
    token::direct_transfer(module_owner, receiver, token_id, 1);

    let (creator_address, collection, name) = token::get_token_data_id_fields(&module_data.token_data_id);
    token::mutate_token_properties(
        module_owner,
        signer::address_of(receiver),
        creator_address,
        collection,
        name,
        0,
        1,
        // Mutate the properties to record the receiveer's address.
        vector<String>[string::utf8(b"given_to")],
        vector<vector<u8>>[bcs::to_bytes(&signer::address_of(receiver))],
        vector<String>[ string::utf8(b"address") ],
    );
}
}
