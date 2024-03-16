namespace StreetTreeApi

type TreeId = {
   tree_id: int64
}

type Tree = {
    created_at: string
    tree_id: int64
    block_id: int64
    the_geom: string
    tree_dbh: int64
    stump_diam: int64
    curb_loc: string
    status: string
    health: string
    spc_latin: string
    spc_common: string
    steward: string
    guards: string
    sidewalk: string
    user_type: string
    problems: string
    root_stone: string
    root_grate: string
    root_other: string
    trnk_wire: string
    trnk_light: string
    trnk_other: string
    brnch_ligh: string
    brnch_shoe: string
    brnch_othe: string
    address: string
    zipcode: int64
    zip_city: string
    cb_num: int64
    borocode: int64
    boroname: string
    cncldist: int64
    st_assem: int64
    st_senate: int64
    nta: string
    nta_name: string
    boro_ct: int64
    state: string
    Latitude: double
    longitude: double
    x_sp: double
    y_sp: double
}
